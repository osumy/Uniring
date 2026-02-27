@model Uniring.Contracts.Ring.RingResponse

@{
    Layout = null; // برای جلوگیری از تداخل استایل‌ها
}

< !doctype html >
    <html lang="fa" dir="rtl">
        <head>
            <meta charset="utf-8" />
            <meta name="viewport" content="width=device-width,initial-scale=1" />
            <title>اطلاعات انگشتر - @Model.Serial</title>
            <link href="https://fonts.googleapis.com/css2?family=Tajawal:wght@300;400;600;700&family=Vazirmatn:wght@300;400;600;700&display=swap" rel="stylesheet">
                <link href="~/css/SearchPageStyle.css" rel="stylesheet" />
                <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.0/css/all.min.css">
                </head>
                <body>
                    <div class="lang-switch" aria-hidden="false" role="toolbar" aria-label="language switch">
                        <div class="lang-group-right">
                            <button class="lang-btn active" data-lang="fa">فارسی</button>
                            <button class="lang-btn" data-lang="ar">العربية</button>
                        </div>
                        <div class="login-group-left">
                            <a href="/login"><button class="login-btn">ورود</button></a>
                        </div>
                    </div>

                    <main class="page" id="page">
                        <div class="logo-circle" aria-hidden="true">
                            <svg viewBox="0 0 24 24" aria-hidden="true">
                                <path d="M15.5 14h-.79l-.28-.27A6.471 6.471 0 0016 9.5 6.5 6.5 0 109.5 16c1.61 0 3.09-.59 4.23-1.57l.27.28v.79l5 4.99L20.49 19l-4.99-5zM10 15a5 5 0 110-10 5 5 0 010 10z" />
                            </svg>
                        </div>

                        <h1 id="title">اطلاعات انگشتر</h1>
                        <p class="lead" id="subtitle">اطلاعات اصالت و مالکیت انگشتر</p>

                        <section id="result-product" class="card" aria-live="polite">
                            <div class="card-content">
                                <div class="badge">
                                    <span class="crown" aria-hidden="true">💍</span>
                                    <span id="prod-title">اطلاعات انگشتر</span>
                                </div>

                                <div class="product-grid" style="gap:22px;align-items:center;margin-top:6px; display: flex; flex-direction: column;">

                                    <div class="prod-image-wrap" style="position: relative; width: 100%; max-width: 400px; height: 350px; overflow: hidden; border-radius: 8px;">
                                        <div id="mediaCarousel" class="media-carousel" style="display: flex; height: 100%; width: 100%; transition: transform 0.3s ease-in-out;">
                                            @if (Model.MediaIds != null && Model.MediaIds.Any())
                                            {
                                                foreach(var mediaId in Model.MediaIds)
                                            {
                                                <div class="media-item" style="flex: 0 0 100%; width: 100%; height: 100%; display: flex; justify-content: center; align-items: center;">
                                                    <img src="http://127.0.0.1:5001/api/Media/download/@mediaId"
                                                        style="max-width: 100%; max-height: 100%; object-fit: contain; border-radius: 8px;">
                                                </div>
                                            }
                            }
                                        </div>
                        
                        @if (Model.MediaIds != null && Model.MediaIds.Count > 1)
                                        {
                            <button class="carousel-btn prev" id="carouselPrev" style="position: absolute; top: 50%; right: 10px; transform: translateY(-50%); z-index: 10; background: rgba(0,0,0,0.5); color: white; border: none; padding: 10px; border-radius: 50%; cursor: pointer;">
                                <i class="fas fa-chevron-right"></i>
                            </button>
                            <button class="carousel-btn next" id="carouselNext" style="position: absolute; top: 50%; left: 10px; transform: translateY(-50%); z-index: 10; background: rgba(0,0,0,0.5); color: white; border: none; padding: 10px; border-radius: 50%; cursor: pointer;">
                                <i class="fas fa-chevron-left"></i>
                            </button>
                                        }
                                    </div>

                                    <div class="prod-info" style="text-align: right; direction: rtl; width: 100%; max-width: 400px; padding: 0 15px;">
                                        <h2 id="prod-name" style="margin:0 0 8px;font-size:26px; color:white;">@Model.Name</h2>
                                        <p id="prod-desc" style="color:#b3b3b3;margin:0 0 12px;">@Model.Description</p>

                                        <dl style="text-align:right;margin:12px 0;color:#b3b3b3;">
                                            <dt style="font-weight:700; color:white;">شماره سریال</dt>
                                            <dd id="prod-serial" style="margin:6px 0 10px; font-family: monospace;">@Model.Serial</dd>
                                        </dl>

                                        <p id="prod-verify" style="margin-top:12px;color:#cbe6d9;font-weight:700;">
                                            اصالت این انگشتر توسط فروشگاه تایید می شود.
                                        </p>

                                        <div style="margin-top:18px;">
                                            <a href="/">
                                                <button class="btn">
                                                    <span id="rtn-btn">بازگشت</span>
                                                </button>
                                            </a>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </section>
                    </main>

                    <script src="~/js/SearchPageScript.js"></script>
                    <script>
        // حالا که DOM بارگذاری شد، کاروسل باید با همان JS اصلی کار کند
                        // اگر لازم بود، اینجا فقط منطق RTL را کمی تنظیم کنید
                        const carousel = document.getElementById('mediaCarousel');
                        const mediaItems = document.querySelectorAll('.media-item');
        if (mediaItems.length > 0) {
                            // فقط اولین تصویر در ابتدا نشان داده شود
                            carousel.style.transform = `translateX(0%)`;
        }
                    </script>
                </body>
            </html>