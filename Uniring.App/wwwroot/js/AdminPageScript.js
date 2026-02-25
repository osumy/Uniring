document.addEventListener('DOMContentLoaded', () => {
    const btnUsers = document.getElementById('action-list-customers');
    const btnRings = document.getElementById('action-list-rings');
    const tableTitle = document.getElementById('dynamicTableTitle');
    const tableHead = document.getElementById('dynamicTableHead');
    const tableBody = document.getElementById('dynamicTableBody');

    if (!btnUsers || !btnRings || !tableBody) return;

    // توابع کمکی
    function setTable(title, headers, rowsHtml) {
        tableTitle.textContent = title;
        tableHead.innerHTML = headers;
        tableBody.innerHTML = rowsHtml;
    }

    function showError(message) {
        setTable('خطا', '<tr><th>خطا</th></tr>', `<tr><td class="error-cell">${message}</td></tr>`);
    }

    function showEmpty(title, headers, message = 'داده‌ای یافت نشد.') {
        setTable(title, headers, `<tr><td class="empty-cell" colspan="10">${message}</td></tr>`);
    }

    // ======================
    // بارگذاری لیست کاربران
    // ======================
    btnUsers.addEventListener('click', async () => {
        const headers = `
            <tr>
                <th>نام نمایشی</th>
                <th>شماره تلفن</th>
                <th>عملیات</th>
            </tr>
        `;

        try {
            const res = await fetch('/api/users');
            if (!res.ok) throw new Error('دریافت داده با خطا مواجه شد.');

            const users = await res.json();

            if (!Array.isArray(users) || users.length === 0) {
                showEmpty('لیست مشتریان', headers);
                return;
            }

            let rows = '';
            users.forEach(u => {
                const id = u.id || '';
                const displayName = u.displayName || '—';
                const phone = u.phoneNumber || '—';

                rows += `
                    <tr data-user-id="${escapeHtml(id)}">
                        <td data-field="displayName">${escapeHtml(displayName)}</td>
                        <td data-field="phoneNumber">${escapeHtml(phone)}</td>
                        <td>
                            <div class="row-actions" data-user-id="${escapeHtml(id)}">
                                <button type="button" class="btn-inline btn-secondary" data-action="orders">سفارشات</button>
                                <button type="button" class="btn-inline btn-primary" data-action="edit">ویرایش</button>
                                <button type="button" class="btn-inline btn-danger" data-action="delete">حذف</button>
                            </div>
                        </td>
                    </tr>
                `;
            });

            setTable('لیست مشتریان', headers, rows);
        } catch (err) {
            console.error('Error loading users:', err);
            showError('خطا در بارگذاری لیست مشتریان. لطفاً دوباره تلاش کنید.');
        }
    });

    // ======================
    // بارگذاری لیست انگشترها
    // ======================
    btnRings.addEventListener('click', async () => {
        const headers = `
            <tr>
                <th>نام</th>
                <th>سریال</th>
                <th>UID</th>
                <th>توضیحات</th>
            </tr>
        `;

        try {
            const res = await fetch('/api/rings');
            if (!res.ok) throw new Error('دریافت داده با خطا مواجه شد.');

            const rings = await res.json();

            if (!Array.isArray(rings) || rings.length === 0) {
                showEmpty('لیست انگشترها', headers);
                return;
            }

            let rows = '';
            rings.forEach(r => {
                rows += `
                    <tr>
                        <td>${escapeHtml(r.name || '—')}</td>
                        <td>${escapeHtml(r.serial || '—')}</td>
                        <td>${escapeHtml(r.uid || '—')}</td>
                        <td>${escapeHtml(r.description || '—')}</td>
                    </tr>
                `;
            });

            setTable('لیست انگشترها', headers, rows);
        } catch (err) {
            console.error('Error loading rings:', err);
            showError('خطا در بارگذاری لیست انگشترها. لطفاً دوباره تلاش کنید.');
        }
    });

    // جلوگیری از XSS
    function escapeHtml(text) {
        if (typeof text !== 'string') return text;
        const map = {
            '&': '&amp;',
            '<': '&lt;',
            '>': '&gt;',
            '"': '&quot;',
            "'": '&#039;'
        };
        return text.replace(/[&<>"']/g, m => map[m]);
    }

    // عملیات روی هر ردیف مشتری (حذف / ویرایش / سفارشات)
    tableBody.addEventListener('click', async (event) => {
        const btn = event.target.closest('.btn-inline');
        if (!btn) return;

        const action = btn.dataset.action;
        const row = btn.closest('tr');
        const container = btn.closest('.row-actions');
        const userId = container?.dataset.userId || row?.dataset.userId;

        if (!userId) {
            alert('شناسه مشتری نامعتبر است.');
            return;
        }

        if (action === 'delete') {
            const ok = confirm('آیا از حذف این مشتری مطمئن هستید؟ این عمل قابل بازگشت نیست.');
            if (!ok) return;

            try {
                const res = await fetch(`/api/users/${encodeURIComponent(userId)}`, {
                    method: 'DELETE'
                });

                if (!res.ok) throw new Error('Delete failed');

                row?.remove();
            } catch (err) {
                console.error('Error deleting user:', err);
                alert('خطا در حذف مشتری. لطفاً دوباره تلاش کنید.');
            }
        } else if (action === 'edit') {
            const displayNameCell = row.querySelector('[data-field="displayName"]');
            const phoneCell = row.querySelector('[data-field="phoneNumber"]');

            const currentName = displayNameCell?.textContent?.trim() || '';
            const currentPhone = phoneCell?.textContent?.trim() || '';

            const newName = prompt('نام نمایشی جدید را وارد کنید:', currentName);
            if (newName === null) return;

            const newPhone = prompt('شماره تلفن جدید را وارد کنید:', currentPhone);
            if (newPhone === null) return;

            const payload = {
                displayName: newName.trim(),
                phoneNumber: newPhone.trim()
            };

            try {
                const res = await fetch(`/api/users/${encodeURIComponent(userId)}`, {
                    method: 'PUT',
                    headers: {
                        'Content-Type': 'application/json'
                    },
                    body: JSON.stringify(payload)
                });

                if (!res.ok) throw new Error('Update failed');

                const updated = await res.json();
                if (displayNameCell) displayNameCell.textContent = updated.displayName || payload.displayName;
                if (phoneCell) phoneCell.textContent = updated.phoneNumber || payload.phoneNumber;
            } catch (err) {
                console.error('Error updating user:', err);
                alert('خطا در بروزرسانی مشخصات مشتری. لطفاً دوباره تلاش کنید.');
            }
        } else if (action === 'orders') {
            alert('نمایش سفارشات برای این مشتری هنوز در بک‌اند پیاده‌سازی نشده است.');
        }
    });
});