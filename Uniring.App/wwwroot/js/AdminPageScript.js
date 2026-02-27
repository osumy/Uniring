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
    let allRings = [];
    let currentUserPage = 1;
    let currentRingPage = 1;
    let currentView = 'users'; // 'users' or 'rings'
    const USERS_PAGE_SIZE = 10; // تعداد ردیف در هر صفحه (برای موبایل هم مناسب است)
    let currentFilterText = '';

    if (!btnUsers || !btnRings || !tableBody) return;

    const toastContainer = document.getElementById("toast-container");

    function showToast(message, type = "info") {
        const toast = document.createElement("div");
        toast.className = `toast ${type}`;
        toast.innerHTML = `<span>${message}</span>`;
        toastContainer.appendChild(toast);
        setTimeout(() => {
            toast.style.opacity = "0";
            toast.style.transition = "opacity 0.5s";
            setTimeout(() => toast.remove(), 500);
        }, 3000);
    }

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

    function getFilteredData() {
        const term = currentFilterText.trim().toLowerCase();
        if (currentView === 'users') {
            if (!Array.isArray(allUsers) || allUsers.length === 0) return [];
            if (!term) return allUsers;
            return allUsers.filter(u => {
                const name = (u.displayName || '').toString().toLowerCase();
                return name.includes(term);
            });
        } else {
            if (!Array.isArray(allRings) || allRings.length === 0) return [];
            if (!term) return allRings;
            return allRings.filter(r => {
                const name = (r.name || '').toString().toLowerCase();
                return name.includes(term);
            });
        }
    }

    function renderUsersPage(pageNumber) {
        const users = getFilteredData();

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
        renderPagination(currentUserPage, totalPages, totalItems, renderUsersPage);
    }

    function renderRingsPage(pageNumber) {
        const rings = getFilteredData();

        const headers = `
            <tr>
                <th>شناسه یکتا (Uid)</th>
                <th>نام</th>
                <th>توضیحات</th>
                <th>سریال</th>
                <th>عملیات</th>
            </tr>
        `;

        if (!Array.isArray(rings) || rings.length === 0) {
            const msg = currentFilterText.trim()
                ? 'هیچ انگشتری مطابق جست‌وجو یافت نشد.'
                : 'انگشتری یافت نشد.';
            showEmpty('لیست انگشترها', headers, msg);
            return;
        }

        const totalItems = rings.length;
        const totalPages = Math.max(1, Math.ceil(totalItems / USERS_PAGE_SIZE));

        currentRingPage = Math.min(Math.max(1, pageNumber), totalPages);

        const start = (currentRingPage - 1) * USERS_PAGE_SIZE;
        const end = start + USERS_PAGE_SIZE;
        const pageItems = rings.slice(start, end);

        let rows = '';
        pageItems.forEach(r => {
            const id = r.id || '';
            const uid = r.uid || '—';
            const name = r.name || '—';
            const desc = r.description || '—';
            const serial = r.serial || '—';

            rows += `
                <tr data-ring-id="${escapeHtml(id)}">
                    <td data-field="uid">${escapeHtml(uid)}</td>
                    <td data-field="name">${escapeHtml(name)}</td>
                    <td data-field="description">${escapeHtml(desc)}</td>
                    <td data-field="serial">${escapeHtml(serial)}</td>
                    <td>
                        <div class="row-actions" data-ring-id="${escapeHtml(id)}">
                            <button type="button" class="btn-inline btn-primary" data-action="edit-ring">ویرایش</button>
                            <button type="button" class="btn-inline btn-danger" data-action="delete-ring">حذف</button>
                        </div>
                    </td>
                </tr>
            `;
        });

        setTable('لیست انگشترها', headers, rows);
        renderPagination(currentRingPage, totalPages, totalItems, renderRingsPage);
    }

    function renderPagination(currentPage, totalPages, totalItems, renderFunc) {
        if (paginationContainer) {
            const canPrev = currentPage > 1;
            const canNext = currentPage < totalPages;

            paginationContainer.innerHTML = `
                <div class="pagination-inner">
                    <button type="button" class="pager-btn" data-page="prev" ${canPrev ? '' : 'disabled'}>قبلی</button>
                    <span class="pager-info">صفحه ${currentPage} از ${totalPages}</span>
                    <button type="button" class="pager-btn" data-page="next" ${canNext ? '' : 'disabled'}>بعدی</button>
                </div>
            `;

            const prevBtn = paginationContainer.querySelector('[data-page="prev"]');
            const nextBtn = paginationContainer.querySelector('[data-page="next"]');

            if (prevBtn) {
                prevBtn.addEventListener('click', () => {
                    if (currentPage > 1) {
                        renderFunc(currentPage - 1);
                    }
                });
            }

            if (nextBtn) {
                nextBtn.addEventListener('click', () => {
                    if (currentPage < totalPages) {
                        renderFunc(currentPage + 1);
                    }
                });
            }
        }
    }

    // ======================
    // بارگذاری لیست کاربران
    // ======================
    btnUsers.addEventListener('click', async () => {
        currentView = 'users';
        if (userSearchInput) userSearchInput.placeholder = "جست‌وجوی مشتری بر اساس نام...";
        try {
            const res = await fetch('/api/users');
            if (!res.ok) throw new Error('دریافت داده با خطا مواجه شد.');

            const users = await res.json();
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
        currentView = 'rings';
        if (userSearchInput) userSearchInput.placeholder = "جست‌وجوی انگشتر بر اساس نام...";
        try {
            const res = await fetch('/api/rings');
            if (!res.ok) throw new Error('دریافت داده با خطا مواجه شد.');

            const rings = await res.json();
            allRings = rings;
            currentFilterText = userSearchInput ? userSearchInput.value || '' : '';
            renderRingsPage(1);
        } catch (err) {
            console.error('Error loading rings:', err);
            showError('خطا در بارگذاری لیست انگشترها. لطفاً دوباره تلاش کنید.');
        }
    });

    if (userSearchInput) {
        userSearchInput.addEventListener('input', () => {
            currentFilterText = userSearchInput.value || '';
            if (currentView === 'users') {
                if (!allUsers.length) return;
                renderUsersPage(1);
            } else {
                if (!allRings.length) return;
                renderRingsPage(1);
            }
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
        const ringId = container?.dataset.ringId || row?.dataset.ringId;

        if (action === 'delete') {
            if (!userId) return;
            openDeleteModal(userId, 'user');
        } else if (action === 'delete-ring') {
            if (!ringId) return;
            openDeleteModal(ringId, 'ring');
        } else if (action === 'edit') {
            window.location.href = `/admin-panel/users/${encodeURIComponent(userId)}/edit`;
        } else if (action === 'edit-ring') {
            showToast('ویرایش انگشتر هنوز پیاده‌سازی نشده است.', 'info');
        } else if (action === 'password') {
            window.location.href = `/admin-panel/users/${encodeURIComponent(userId)}/change-password`;
        } else if (action === 'orders') {
            showToast('نمایش سفارشات برای این مشتری هنوز در بک‌اند پیاده‌سازی نشده است.', 'info');
        }
    });

    // ======================
    // مودال حذف
    // ======================
    const deleteModal = document.getElementById('deleteModal');
    const deleteModalConfirm = document.getElementById('deleteConfirmBtn');
    const deleteModalCancel = document.getElementById('deleteCancelBtn');
    const modalTitle = deleteModal?.querySelector('.modal-title');
    const modalText = deleteModal?.querySelector('.modal-text');
    let deleteTargetId = null;
    let deleteTargetType = null; // 'user' or 'ring'

    function openDeleteModal(id, type) {
        deleteTargetId = id;
        deleteTargetType = type;
        if (!deleteModal) return;

        if (type === 'user') {
            if (modalTitle) modalTitle.textContent = 'حذف مشتری';
            if (modalText) modalText.textContent = 'آیا از حذف این مشتری مطمئن هستید؟ این عمل قابل بازگشت نیست.';
        } else {
            if (modalTitle) modalTitle.textContent = 'حذف انگشتر';
            if (modalText) modalText.textContent = 'آیا از حذف این انگشتر مطمئن هستید؟ این عمل قابل بازگشت نیست.';
        }

        deleteModal.classList.add('is-open');
    }

    function closeDeleteModal() {
        if (!deleteModal) return;
        deleteTargetId = null;
        deleteTargetType = null;
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
            if (!deleteTargetId) {
                closeDeleteModal();
                return;
            }
            const id = deleteTargetId;
            const type = deleteTargetType;
            const url = type === 'user' ? `/api/users/${encodeURIComponent(id)}` : `/api/rings/${encodeURIComponent(id)}`;

            try {
                const res = await fetch(url, {
                    method: 'DELETE'
                });

                if (!res.ok) throw new Error('Delete failed');

                if (type === 'user') {
                    allUsers = allUsers.filter(u => u.id !== id);
                    const maxPage = Math.max(1, Math.ceil(allUsers.length / USERS_PAGE_SIZE));
                    if (currentUserPage > maxPage) currentUserPage = maxPage;
                    renderUsersPage(currentUserPage);
                } else {
                    allRings = allRings.filter(r => r.id !== id);
                    const maxPage = Math.max(1, Math.ceil(allRings.length / USERS_PAGE_SIZE));
                    if (currentRingPage > maxPage) currentRingPage = maxPage;
                    renderRingsPage(currentRingPage);
                }

                closeDeleteModal();
                showToast('با موفقیت حذف شد.', 'success');
            } catch (err) {
                console.error('Error deleting:', err);
                showToast('خطا در حذف. لطفاً دوباره تلاش کنید.', 'error');
                closeDeleteModal();
            }
        });
    }
});