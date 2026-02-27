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
    const dtUid = document.getElementById('prod-dt-uid');
    const dtSerial = document.getElementById('prod-dt-serial');

    if (dtUid) dtUid.textContent = s.labelUid || (s.lang === 'ar' ? 'معرف فريد' : 'شناسه یکتا (Uid)');
    if (dtSerial) dtSerial.textContent = s.labelSerial || (s.lang === 'ar' ? 'الرقم التسلسلي' : 'شماره سریال');

    // update active state on language buttons
    langBtns.forEach(b => b.classList.toggle('active', b.dataset.lang === langKey));

    // switch font: Vazirmatn for Persian prefer, Tajawal for Arabic
    if (langKey === 'ar') {
        document.body.style.fontFamily = '"Tajawal","Vazirmatn", system-ui, -apple-system, "Segoe UI", Roboto, Arial';
    } else {
        document.body.style.fontFamily = '"Vazirmatn","Tajawal", system-ui, -apple-system, "Segoe UI", Roboto, Arial';
    }

    // If a product is already rendered, re-localize its dynamic parts (purchase date formatting)
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
// product = { imageUrl, name, description, purchaseISO, serial }
let currentMediaIndex = 0;
let mediaItems = [];

function renderProduct(product) {
    hideResults();

    // Fill fields
    document.getElementById('prod-name').textContent = product.name || '';
    document.getElementById('prod-desc').textContent = product.description || '';
    document.getElementById('prod-uid').textContent = product.uid || '';
    document.getElementById('prod-serial').textContent = product.serial || '';

    // Populate media carousel
    const mediaCarousel = document.getElementById('mediaCarousel');
    mediaCarousel.innerHTML = ''; // Clear previous media
    mediaItems = [];

    if (product.mediaIds && product.mediaIds.length > 0) {
        product.mediaIds.forEach(mediaId => {
            const mediaUrl = `/Media/download/${mediaId}`;
            const mediaItemDiv = document.createElement('div');
            mediaItemDiv.className = 'media-item';

            // Determine if it's an image or video based on extension (or content type from API if available)
            // For simplicity, let's assume if it ends with mp4, webm, mov, mkv it's a video
            // In a real app, you'd get content type from API
            const isVideo = mediaUrl.match(/\.(mp4|webm|mov|mkv)$/i);

            if (isVideo) {
                mediaItemDiv.innerHTML = `<video controls src="${mediaUrl}" style="width: 100%; height: 100%; object-fit: contain; border-radius: 8px;"></video>`;
            } else {
                mediaItemDiv.innerHTML = `<img src="${mediaUrl}" alt="ring image" style="width: 100%; height: 100%; object-fit: contain; border-radius: 8px;">`;
            }
            mediaCarousel.appendChild(mediaItemDiv);
            mediaItems.push(mediaItemDiv);
        });

        // Show/hide carousel buttons
        if (mediaItems.length > 1) {
            document.getElementById('carouselPrev').style.display = 'flex';
            document.getElementById('carouselNext').style.display = 'flex';
        } else {
            document.getElementById('carouselPrev').style.display = 'none';
            document.getElementById('carouselNext').style.display = 'none';
        }
        showMedia(0);
    } else {
        // No media, show a placeholder
        mediaCarousel.innerHTML =
            `<div class="media-item">
                <img src="https://via.placeholder.com/800x600.png?text=No+Image" alt="No image available" style="width: 100%; height: 100%; object-fit: contain; border-radius: 8px;">
            </div>`;
        document.getElementById('carouselPrev').style.display = 'none';
        document.getElementById('carouselNext').style.display = 'none';
    }

    // Check login status and display prompt
    const loginPrompt = document.getElementById('login-prompt');
    // This is a placeholder for actual login check. 
    // In a real application, you would check for an authentication cookie or token.
    const isLoggedIn = false; // Replace with actual login check

    if (!isLoggedIn) {
        loginPrompt.style.display = 'block';
        const loginLink = loginPrompt.querySelector('a');
        const currentLang = document.documentElement.getAttribute('lang');
        if (currentLang === 'ar') {
            loginLink.href = '/Account/LoginAr'; // Assuming Arabic login page
        } else {
            loginLink.href = '/Account/Login'; // Default Persian/English login page
        }
    } else {
        loginPrompt.style.display = 'none';
    }

    // verification sentence localized
    const cur = document.documentElement.getAttribute('lang') === 'ar' ? 'ar' : 'fa';
    document.getElementById('prod-verify').textContent = cur === 'ar'
        ? 'تم التحقق من أصالة هذا الخاتم بواسطة المتجر.'
        : 'اصالت این انگشتر توسط فروشگاه تایید می شود.';

    document.getElementById('prod-btn').textContent = cur === 'ar' ? 'رجوع' : 'بازگشت';

    // show product section
    document.getElementById('result-product').style.display = 'block';
    window.scrollTo({ top: document.getElementById('result-product').offsetTop - 20, behavior: 'smooth' });
}

function showMedia(index) {
    if (mediaItems.length === 0) return;
    currentMediaIndex = (index + mediaItems.length) % mediaItems.length;
    const carousel = document.getElementById('mediaCarousel');
    carousel.style.transform = `translateX(-${currentMediaIndex * 100}%)`;
}

document.getElementById('carouselPrev').addEventListener('click', () => {
    showMedia(currentMediaIndex - 1);
});

document.getElementById('carouselNext').addEventListener('click', () => {
    showMedia(currentMediaIndex + 1);
});

/* Replace your current searchSerial() body with this logic or call this from it.
   Example logic: call your backend; if 404 -> renderNotFound(); else renderProduct(json)
*/
// Replace existing searchSerial() with this async implementation
async function searchIdentifier() {
    const val = input.value.trim();
    if (!val) {
        input.animate(
            [{ transform: 'translateX(0)' }, { transform: 'translateX(-6px)' }, { transform: 'translateX(6px)' }, { transform: 'translateX(0)' }],
            { duration: 300 }
        );
        return;
    }

    // Show a quick loading state on the button (optional)
    const btn = document.querySelector('.search-row .btn');
    const oldBtnText = btn.querySelector('span').textContent;
    btn.disabled = true;
    btn.style.opacity = '0.8';
    btn.querySelector('span').textContent = document.documentElement.getAttribute('lang') === 'ar' ? 'جارٍ البحث...' : 'در حال جستجو...';

    try {
        // Use relative URL; adjust base if your API is on a different host/port
        const res = await fetch(`/api/ring/${encodeURIComponent(val)}`);

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
        // {"uid":"UID","name":"انگشتر عقیق","serial":"R2732874204","description":null}

        // Map server JSON to the product object expected by renderProduct()
        const product = {
            name: data.name || '',
            description: data.description || '',
            uid: data.uid || '',
            serial: data.serial || '',
            mediaIds: data.mediaIds || []
        };

        // render the product
        const isLoggedIn = document.cookie.split(';').some((item) => item.trim().startsWith('authToken=')); // Check for login cookie
        renderProduct(product, isLoggedIn);

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

