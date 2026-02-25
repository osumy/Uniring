document.addEventListener('DOMContentLoaded', () => {
    const btnUsers = document.getElementById('action-list-customers');
    const btnRings = document.getElementById('action-list-rings');
    const tableTitle = document.getElementById('dynamicTableTitle');
    const tableHead = document.getElementById('dynamicTableHead');
    const tableBody = document.getElementById('dynamicTableBody');
    const paginationContainer = document.getElementById('paginationContainer');
    const userSearchInput = document.getElementById('userSearchInput');

    // state for users list + pagination
    let allUsers = [];
    let currentUserPage = 1;
    const USERS_PAGE_SIZE = 10; // تعداد ردیف در هر صفحه (برای موبایل هم مناسب است)
    let currentFilterText = '';

    if (!btnUsers || !btnRings || !tableBody) return;

    // توابع کمکی
    function setTable(title, headers, rowsHtml) {
        tableTitle.textContent = title;
        tableHead.innerHTML = headers;
        tableBody.innerHTML = rowsHtml;
        if (paginationContainer) {
            paginationContainer.innerHTML = '';
        }
    }

    function showError(message) {
        setTable('خطا', '<tr><th>خطا</th></tr>', `<tr><td class="error-cell">${message}</td></tr>`);
    }

    function showEmpty(title, headers, message = 'داده‌ای یافت نشد.') {
        setTable(title, headers, `<tr><td class="empty-cell" colspan="10">${message}</td></tr>`);
    }

    function formatPhoneForDisplay(rawPhone) {
        if (typeof rawPhone !== 'string' || !rawPhone) return '';

        // اگر شماره در قالب E.164 ایران باشد (مثل +9891xxxxxxx)، آن را به فرمت داخلی 09.. تبدیل می‌کنیم
        if (rawPhone.startsWith('+98') && rawPhone.length >= 4) {
            return '0' + rawPhone.substring(3);
        }

        // در غیر این صورت همان مقدار را نشان بده
        return rawPhone;
    }

    function getFilteredUsers() {
        if (!Array.isArray(allUsers) || allUsers.length === 0) return [];

        const term = currentFilterText.trim().toLowerCase();
        if (!term) return allUsers;

        return allUsers.filter(u => {
            const name = (u.displayName || '').toString().toLowerCase();
            return name.includes(term);
        });
    }

    function renderUsersPage(pageNumber) {
        const users = getFilteredUsers();

        const headers = `
            <tr>
                <th>نام نمایشی</th>
                <th>شماره تلفن</th>
                <th>عملیات</th>
            </tr>
        `;

        if (!Array.isArray(users) || users.length === 0) {
            const msg = currentFilterText.trim()
                ? 'هیچ مشتری مطابق جست‌وجو یافت نشد.'
                : 'مشتری‌ای یافت نشد.';
            showEmpty('لیست مشتریان', headers, msg);
            return;
        }

        const totalItems = users.length;
        const totalPages = Math.max(1, Math.ceil(totalItems / USERS_PAGE_SIZE));

        currentUserPage = Math.min(Math.max(1, pageNumber), totalPages);

        const start = (currentUserPage - 1) * USERS_PAGE_SIZE;
        const end = start + USERS_PAGE_SIZE;
        const pageItems = users.slice(start, end);

        let rows = '';
        pageItems.forEach(u => {
            const id = u.id || '';
            const displayName = u.displayName || '—';
            const phone = formatPhoneForDisplay(u.phoneNumber || '');

            rows += `
                <tr data-user-id="${escapeHtml(id)}">
                    <td data-field="displayName">${escapeHtml(displayName)}</td>
                    <td data-field="phoneNumber">${escapeHtml(phone || '—')}</td>
                    <td>
                        <div class="row-actions" data-user-id="${escapeHtml(id)}">
                            <button type="button" class="btn-inline btn-secondary" data-action="orders">انگشترها</button>
                            <button type="button" class="btn-inline btn-primary" data-action="edit">ویرایش</button>
                            <button type="button" class="btn-inline btn-primary" data-action="password">ویرایش رمز عبور</button>
                            <button type="button" class="btn-inline btn-danger" data-action="delete">حذف</button>
                        </div>
                    </td>
                </tr>
            `;
        });

        setTable('لیست مشتریان', headers, rows);

        // رندر کنترل‌های صفحه‌بندی
        if (paginationContainer) {
            const canPrev = currentUserPage > 1;
            const canNext = currentUserPage < totalPages;

            paginationContainer.innerHTML = `
                <div class="pagination-inner">
                    <button type="button" class="pager-btn" data-page="prev" ${canPrev ? '' : 'disabled'}>قبلی</button>
                    <span class="pager-info">صفحه ${currentUserPage} از ${totalPages}</span>
                    <button type="button" class="pager-btn" data-page="next" ${canNext ? '' : 'disabled'}>بعدی</button>
                </div>
            `;

            const prevBtn = paginationContainer.querySelector('[data-page="prev"]');
            const nextBtn = paginationContainer.querySelector('[data-page="next"]');

            if (prevBtn) {
                prevBtn.addEventListener('click', () => {
                    if (currentUserPage > 1) {
                        renderUsersPage(currentUserPage - 1);
                    }
                });
            }

            if (nextBtn) {
                nextBtn.addEventListener('click', () => {
                    const totalPages2 = Math.max(1, Math.ceil(totalItems / USERS_PAGE_SIZE));
                    if (currentUserPage < totalPages2) {
                        renderUsersPage(currentUserPage + 1);
                    }
                });
            }
        }
    }

    // ======================
    // بارگذاری لیست کاربران
    // ======================
    btnUsers.addEventListener('click', async () => {
        try {
            const res = await fetch('/api/users');
            if (!res.ok) throw new Error('دریافت داده با خطا مواجه شد.');

            const users = await res.json();

            if (!Array.isArray(users) || users.length === 0) {
                const headers = `
                    <tr>
                        <th>نام نمایشی</th>
                        <th>شماره تلفن</th>
                        <th>عملیات</th>
                    </tr>
                `;
                showEmpty('لیست مشتریان', headers);
                return;
            }

            allUsers = users;
            currentFilterText = userSearchInput ? userSearchInput.value || '' : '';
            renderUsersPage(1);
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

    if (userSearchInput) {
        userSearchInput.addEventListener('input', () => {
            currentFilterText = userSearchInput.value || '';
            if (!allUsers.length) return;
            renderUsersPage(1);
        });
    }

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

    // عملیات روی هر ردیف مشتری (حذف / ویرایش / رمز / سفارشات)
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
            // حذف در مودال اختصاصی هندل می‌شود
            openDeleteModal(userId);
        } else if (action === 'edit') {
            // ویرایش مشخصات در صفحه جداگانه
            window.location.href = `/admin-panel/users/${encodeURIComponent(userId)}/edit`;
        } else if (action === 'password') {
            // ویرایش رمز عبور در صفحه جداگانه
            window.location.href = `/admin-panel/users/${encodeURIComponent(userId)}/change-password`;
        } else if (action === 'orders') {
            alert('نمایش سفارشات برای این مشتری هنوز در بک‌اند پیاده‌سازی نشده است.');
        }
    });

    // ======================
    // مودال حذف کاربر
    // ======================
    const deleteModal = document.getElementById('deleteModal');
    const deleteModalConfirm = document.getElementById('deleteConfirmBtn');
    const deleteModalCancel = document.getElementById('deleteCancelBtn');
    let deleteUserId = null;

    function openDeleteModal(userId) {
        deleteUserId = userId;
        if (!deleteModal) return;
        deleteModal.classList.add('is-open');
    }

    function closeDeleteModal() {
        if (!deleteModal) return;
        deleteUserId = null;
        deleteModal.classList.remove('is-open');
    }

    if (deleteModalCancel) {
        deleteModalCancel.addEventListener('click', () => {
            closeDeleteModal();
        });
    }

    if (deleteModal) {
        deleteModal.addEventListener('click', (e) => {
            if (e.target === deleteModal) {
                closeDeleteModal();
            }
        });
    }

    if (deleteModalConfirm) {
        deleteModalConfirm.addEventListener('click', async () => {
            if (!deleteUserId) {
                closeDeleteModal();
                return;
            }
            const id = deleteUserId;
            try {
                const res = await fetch(`/api/users/${encodeURIComponent(id)}`, {
                    method: 'DELETE'
                });

                if (!res.ok) throw new Error('Delete failed');

                // حذف ردیف از جدول و بروزرسانی لیست داخلی
                allUsers = allUsers.filter(u => u.id !== id);
                if (allUsers.length === 0) {
                    const headers = `
                        <tr>
                            <th>نام نمایشی</th>
                            <th>شماره تلفن</th>
                            <th>عملیات</th>
                        </tr>
                    `;
                    showEmpty('لیست مشتریان', headers, 'مشتری‌ای یافت نشد.');
                } else {
                    // اگر صفحه فعلی بعد از حذف خالی شد، یک صفحه برگردیم
                    const maxPage = Math.max(1, Math.ceil(allUsers.length / USERS_PAGE_SIZE));
                    if (currentUserPage > maxPage) {
                        currentUserPage = maxPage;
                    }
                    renderUsersPage(currentUserPage);
                }

                closeDeleteModal();
            } catch (err) {
                console.error('Error deleting user:', err);
                alert('خطا در حذف مشتری. لطفاً دوباره تلاش کنید.');
                closeDeleteModal();
            }
        });
    }
});