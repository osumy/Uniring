const registrations = [
    { id: 'RNG-003-2024', ringName: 'انگشتر تیتانیوم مدرن', customerName: 'دیوید رودریگز', customerEmail: 'david.r@email.com', purchaseDate: '25 ژانویه 2024', price: '$899.99', material: 'TITANIUM' },
    { id: 'RNG-002-2024', ringName: 'حلقه طلای وینتیج', customerName: 'مایکل چن', customerEmail: 'michael.c@email.com', purchaseDate: '20 ژانویه 2024', price: '$1,299.99', material: 'GOLD' },
    { id: 'RNG-001-2024', ringName: 'حلقه کلاسیک الماس', customerName: 'سارا جانسون', customerEmail: 'sarah.j@email.com', purchaseDate: '15 ژانویه 2024', price: '$2,499.99', material: 'PLATINUM' },
    { id: 'RR001236', ringName: 'بافت نقره‌ای گران‌قیمت', customerName: 'امیلی دیویس', customerEmail: 'emily.d@example.com', purchaseDate: '25 ژانویه 2024', price: '$2,800', material: 'SILVER' },
    { id: 'RR001235', ringName: 'تنگستن کارباید قدرتمند', customerName: 'مایکل چن', customerEmail: 'michael.c@example.com', purchaseDate: '20 ژانویه 2024', price: '$6,200', material: 'TUNGSTEN' },
];

// نگاشت کلاس متریال
const matClass = {
    'TITANIUM': 'mat-TITANIUM',
    'GOLD': 'mat-GOLD',
    'PLATINUM': 'mat-PLATINUM',
    'SILVER': 'mat-SILVER',
    'TUNGSTEN': 'mat-TUNGSTEN'
};

const matMap = { 'TITANIUM': 'تیتانیوم', 'GOLD': 'طلا', 'PLATINUM': 'پلاتین', 'SILVER': 'نقره', 'TUNGSTEN': 'تنگستن' };

function renderRows(data) {
    const tbody = document.getElementById('registrationsBody');
    if (!tbody) return;
    tbody.innerHTML = '';
    data.forEach(reg => {
        const tr = document.createElement('tr');

        const td1 = document.createElement('td');
        const pName = document.createElement('p'); pName.className = 'ring-name'; pName.textContent = reg.ringName;
        const pId = document.createElement('p'); pId.className = 'ring-id'; pId.textContent = reg.id;
        td1.appendChild(pName); td1.appendChild(pId);

        const td2 = document.createElement('td');
        const cust = document.createElement('div'); cust.className = 'customer';
        const avatar = document.createElement('div'); avatar.className = 'avatar';
        const initials = reg.customerName.split(' ').map(s => s[0]).slice(0, 2).join('').toUpperCase();
        avatar.textContent = initials;
        const info = document.createElement('div');
        const cname = document.createElement('p'); cname.className = 'cust-name'; cname.textContent = reg.customerName;
        const cemail = document.createElement('p'); cemail.className = 'cust-email'; cemail.textContent = reg.customerEmail;
        info.appendChild(cname); info.appendChild(cemail);
        cust.appendChild(avatar); cust.appendChild(info);
        td2.appendChild(cust);

        const td3 = document.createElement('td');
        const purchase = document.createElement('div'); purchase.className = 'purchase';
        purchase.innerHTML = '<span style="margin-right:6px;">' + reg.purchaseDate + '</span>';
        td3.appendChild(purchase);

        const td4 = document.createElement('td'); td4.className = 'price'; td4.textContent = reg.price;

        const td5 = document.createElement('td');
        const badge = document.createElement('span'); badge.className = 'badge ' + (matClass[reg.material] || '');
        badge.textContent = matMap[reg.material] || reg.material;
        td5.appendChild(badge);

        tr.appendChild(td1); tr.appendChild(td2); tr.appendChild(td3); tr.appendChild(td4); tr.appendChild(td5);
        tbody.appendChild(tr);
    });
}

// اجرا در بارگذاری صفحه
document.addEventListener('DOMContentLoaded', () => {
    renderRows(registrations);
});
