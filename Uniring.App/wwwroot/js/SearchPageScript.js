// Translations for Persian (fa) and Arabic (ar)
const strings = {
    fa: {
        lang: "fa",
        dir: "rtl",
        title: "تأیید اصالت انگشتر",
        subtitle: "شماره سریال انگشتر خود را وارد کنید تا اصالت و اطلاعات مالکیت را بررسی کنید.",
        lookup: "جستجوی شماره سریال",
        placeholder: "شماره سریال انگشتر را وارد کنید (مثلاً: RNG-001-2024)",
        btn: "جستجو",
        help: "در یافتن انگشتر خود مشکل دارید؟ برای کمک با تیم پشتیبانی تماس بگیرید."
    },
    ar: {
        lang: "ar",
        dir: "rtl",
        title: "التحقق من خاتم",
        subtitle: "أدخل الرقم التسلسلي للخاتم للتحقق من الأصالة ومعرفة بيانات الملكية",
        lookup: "بحث الرقم التسلسلي",
        placeholder: "أدخل الرقم التسلسلي للخاتم (مثال: RNG-001-2024)",
        btn: "بحث",
        help: "هل تواجه مشكلة في العثور على خاتمك؟ اتصل بفريق الدعم للحصول على المساعدة."
    }
};

// Elements
const page = document.getElementById('page');
const title = document.getElementById('title');
const subtitle = document.getElementById('subtitle');
const lookup = document.getElementById('lookup-label');
const input = document.getElementById('serial');
const btnText = document.getElementById('btn-text');
const help = document.getElementById('help');
const langBtns = document.querySelectorAll('.lang-btn');

// Set language content
// Replace existing setLang with this enhanced version
function setLang(langKey) {
    const s = strings[langKey] || strings.fa;
    // set dir on document element
    document.documentElement.setAttribute('dir', s.dir);
    document.documentElement.setAttribute('lang', s.lang === 'fa' ? 'fa' : 'ar');

    // update top-level texts
    title.textContent = s.title;
    subtitle.textContent = s.subtitle;
    lookup.textContent = s.lookup;
    input.placeholder = s.placeholder;
    btnText.textContent = s.btn;
    help.textContent = s.help;

    // update result (Not Found) texts
    document.getElementById('nf-title').textContent = s.notFoundTitle || (s.lang === 'ar' ? 'لم يتم العثور' : 'یافت نشد');
    document.getElementById('nf-message').textContent = s.notFoundMessage || (s.lang === 'ar'
        ? 'الرقم التسلسلي غير موجود في نظامنا. يرجى التحقق أو الاتصال بالدعم.'
        : 'شماره سریال مورد نظر در سیستم ما پیدا نشد. لطفا دوباره بررسی کنید یا با پشتیبانی تماس بگیرید.');
    document.getElementById('nf-btn').textContent = s.tryAgain || (s.lang === 'ar' ? 'حاول مرة أخرى' : 'بررسی دوباره');

    // update product card static texts
    document.getElementById('prod-title').textContent = s.productTitle || (s.lang === 'ar' ? 'معلومات الخاتم' : 'اطلاعات انگشتر');
    document.getElementById('prod-verify').textContent = s.verifyText || (s.lang === 'ar' ? 'تم التحقق من أصالة هذا الخاتم بواسطة المتجر' : 'اصالت این انگشتر توسط فروشگاه تایید می شود');
    document.getElementById('prod-btn').textContent = s.backBtn || (s.lang === 'ar' ? 'رجوع' : 'بازگشت');

    // update the dt labels in product details
    const dtPurchase = document.getElementById('prod-dt-purchase');
    const dtSerial = document.getElementById('prod-dt-serial');
    const dtPrice = document.getElementById('prod-dt-price');

    if (dtPurchase) dtPurchase.textContent = s.labelPurchase || (s.lang === 'ar' ? 'تاريخ الشراء' : 'تاریخ خرید');
    if (dtSerial) dtSerial.textContent = s.labelSerial || (s.lang === 'ar' ? 'الرقم التسلسلي' : 'شماره سریال');
    if (dtPrice) dtPrice.textContent = s.labelPrice || (s.lang === 'ar' ? 'السعر' : 'قیمت');

    // update active state on language buttons
    langBtns.forEach(b => b.classList.toggle('active', b.dataset.lang === langKey));

    // switch font: Vazirmatn for Persian prefer, Tajawal for Arabic
    if (langKey === 'ar') {
        document.body.style.fontFamily = '"Tajawal","Vazirmatn", system-ui, -apple-system, "Segoe UI", Roboto, Arial';
    } else {
        document.body.style.fontFamily = '"Vazirmatn","Tajawal", system-ui, -apple-system, "Segoe UI", Roboto, Arial';
    }

    // If a product is already rendered, re-localize its dynamic parts (purchase date & price formatting)
    // Example: if result-product is visible and product data fields are present, re-format them:
    const prodSectionVisible = document.getElementById('result-product').style.display !== 'none';
    if (prodSectionVisible) {
        // reformat purchase date if present
        const purchaseText = document.getElementById('prod-purchase').getAttribute('data-iso');
        if (purchaseText) {
            const dt = new Date(purchaseText);
            const locale = document.documentElement.getAttribute('lang') === 'ar' ? 'ar-EG' : 'fa-IR';
            document.getElementById('prod-purchase').textContent = isNaN(dt) ? '' : dt.toLocaleString(locale);
        }
        // reformat price numeric value if stored
        const priceRaw = document.getElementById('prod-price').getAttribute('data-raw');
        if (priceRaw) {
            try {
                const num = Number(priceRaw);
                if (Number.isFinite(num)) {
                    const locale = document.documentElement.getAttribute('lang') === 'ar' ? 'ar-EG' : 'fa-IR';
                    document.getElementById('prod-price').textContent = new Intl.NumberFormat(locale).format(num) + (s.priceSuffix || ' USD');
                }
            } catch (e) { }
        }
    }

    input.focus();
}


// attach events to language buttons
langBtns.forEach(btn => {
    btn.addEventListener('click', () => setLang(btn.dataset.lang));
});

// emulate a search action
function searchSerial() {
    const val = input.value.trim();
    if (!val) {
        // small shake animation to indicate required
        input.animate([{ transform: 'translateX(0)' }, { transform: 'translateX(-6px)' }, { transform: 'translateX(6px)' }, { transform: 'translateX(0)' }], { duration: 300 });
        return;
    }
    // For the demo just show an alert localized
    const cur = document.documentElement.getAttribute('lang') === 'ar' ? 'ar' : 'fa';
    const msg = cur === 'ar' ? `جارٍ البحث عن: ${val}` : `در حال جستجو: ${val}`;
    alert(msg);
}

// initialize default
setLang('fa');

// Utility: hide all result sections and show only the one we want
function hideResults() {
    document.getElementById('result-notfound').style.display = 'none';
    document.getElementById('result-product').style.display = 'none';
    // restore card visibility (if you want to hide card when showing result, comment the next line)
    document.querySelector('section.card').style.display = 'none';
}

// Render Not Found
function renderNotFound() {
    hideResults();
    // populate localized messages
    const cur = document.documentElement.getAttribute('lang') === 'ar' ? 'ar' : 'fa';
    document.getElementById('nf-title').textContent = cur === 'ar' ? 'لم يتم العثور' : 'یافت نشد';
    document.getElementById('nf-message').textContent = cur === 'ar'
        ? 'الرقم التسلسلي غير موجود في نظامنا. يرجى التحقق أو الاتصال بالدعم.'
        : 'شماره سریال مورد نظر در سیستم ما پیدا نشد. لطفا دوباره بررسی کنید یا با پشتیبانی تماس بگیرید.';
    document.getElementById('nf-btn').textContent = cur === 'ar' ? 'حاول مرة أخرى' : 'بررسی دوباره';

    document.getElementById('result-notfound').style.display = 'block';
    window.scrollTo({ top: document.getElementById('result-notfound').offsetTop - 20, behavior: 'smooth' });
}

// Render Product
// product = { imageUrl, name, description, purchaseISO, serial, price }
function renderProduct(product) {
    hideResults();

    // Fill fields
    document.getElementById('prod-image').src = product.imageUrl || '';
    document.getElementById('prod-image').alt = product.name || 'ring image';

    document.getElementById('prod-name').textContent = product.name || '';
    document.getElementById('prod-desc').textContent = product.description || '';

    // Format datetime — simple localized formatting
    const dt = product.purchaseISO ? new Date(product.purchaseISO) : null;
    const cur = document.documentElement.getAttribute('lang') === 'ar' ? 'ar' : 'fa';
    const locale = cur === 'ar' ? 'ar-EG' : 'fa-IR';
    document.getElementById('prod-purchase').textContent = dt ? dt.toLocaleString(locale) : '';

    document.getElementById('prod-serial').textContent = product.serial || '';
    document.getElementById('prod-price').textContent = product.price || '';

    // verification sentence localized
    document.getElementById('prod-verify').textContent = cur === 'ar'
        ? 'تم التحقق من أصالة هذا الخاتم بواسطة المتجر.'
        : 'اصالت این انگشتر توسط فروشگاه تایید می شود.';

    document.getElementById('prod-btn').textContent = cur === 'ar' ? 'رجوع' : 'بازگشت';

    // show product section
    document.getElementById('result-product').style.display = 'block';
    window.scrollTo({ top: document.getElementById('result-product').offsetTop - 20, behavior: 'smooth' });
}

/* Replace your current searchSerial() body with this logic or call this from it.
   Example logic: call your backend; if 404 -> renderNotFound(); else renderProduct(json)
*/
// Replace existing searchSerial() with this async implementation
async function searchSerial() {
    const val = input.value.trim();
    if (!val) {
        input.animate(
            [{ transform: 'translateX(0)' }, { transform: 'translateX(-6px)' }, { transform: 'translateX(6px)' }, { transform: 'translateX(0)' }],
            { duration: 300 }
        );
        return;
    }

    // Show a quick loading state on the button (optional)
    const btn = document.querySelector('.btn');
    const oldBtnText = btn.querySelector('span').textContent;
    btn.disabled = true;
    btn.style.opacity = '0.8';
    btn.querySelector('span').textContent = document.documentElement.getAttribute('lang') === 'ar' ? 'جارٍ البحث...' : 'در حال جستجو...';

    try {
        // Use relative URL; adjust base if your API is on a different host/port
        const res = await fetch(`/serial/${encodeURIComponent(val)}`, { method: 'GET' });

        if (res.status === 404) {
            // not found
            renderNotFound();
            return;
        }

        if (!res.ok) {
            // other server error
            console.error('Server error', res.status);
            renderNotFound();
            return;
        }

        // parse JSON
        const data = await res.json();
        // Example of expected JSON:
        // {"uid":"UID","name":"انگشتر عقیق","serial":"R2732874204","price":25,"description":null}

        // Map server JSON to the product object expected by renderProduct()
        const product = {
            imageUrl: data.imageUrl || data.image || 'https://via.placeholder.com/800x600.png?text=Ring+Image', // if server provides imageUrl use it
            name: data.name || '',
            description: data.description || '',

            // server didn't send purchase date in your example — keep null or map if available
            purchaseISO: data.purchaseISO || data.purchaseDate || null,

            serial: data.serial || data.uid || val,

            // format price: keep raw numeric price and add a label — adjust to your currency rules
            price: (function (p) {
                if (p === null || p === undefined) return '';
                // add thousands separators for readability
                try {
                    const num = Number(p);
                    if (Number.isFinite(num)) {
                        return new Intl.NumberFormat(document.documentElement.getAttribute('lang') === 'ar' ? 'ar-EG' : 'fa-IR').format(num) + ' USD';
                    }
                } catch (e) { }
                return String(p);
            })(data.price)
        };

        // render the product
        renderProduct(product);

    } catch (err) {
        console.error('Network or parsing error', err);
        renderNotFound();
    } finally {
        // restore button state
        btn.disabled = false;
        btn.style.opacity = '';
        btn.querySelector('span').textContent = oldBtnText;
    }
}

