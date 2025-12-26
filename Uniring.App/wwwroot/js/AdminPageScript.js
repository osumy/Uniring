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
                <th>شناسه کاربری</th>
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
                // فقط کاربران با role = "user" — ولی فیلتر از سمت سرور انجام می‌شه
                rows += `
                    <tr>
                        <td>${escapeHtml(u.displayName || '—')}</td>
                        <td>${escapeHtml(u.phoneNumber || '—')}</td>
                        <td>${escapeHtml(u.id || '—')}</td>
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
});