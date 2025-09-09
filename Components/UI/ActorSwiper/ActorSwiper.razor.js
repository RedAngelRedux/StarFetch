
// // This method is sufficient, but is not bulletproof; It will crash if the DOM is not fully "wet" when it is called.

//export function init(container) {
//    container.querySelector('.nav-btn-next').addEventListener('click', () => {
//        goToNext(container);
//    });

//    container.querySelector('.nav-btn-prev').addEventListener('click', () => {
//        goToPrev(container);
//    });
//}

// // This method is an improvement over the 'minimum' attempt, in that it varifies the DOM is fully rendered before acting
export function init(container) {
    const maxRetries = 10;
    const retryDelay = 200; // milliseconds
    let attempts = 0;

    function tryInit() {
        const nextBtn = container.querySelector('.nav-btn-next');
        const prevBtn = container.querySelector('.nav-btn-prev');

        if (nextBtn && prevBtn) {
            nextBtn.addEventListener('click', () => goToNext(container));
            prevBtn.addEventListener('click', () => goToPrev(container));
            console.log("ActorSwiper initialized.");
        } else if (attempts < maxRetries) {
            attempts++;
            setTimeout(tryInit, retryDelay);
        } else {
            console.warn("ActorSwiper failed to initialize: buttons not found.");
        }
    }

    tryInit();
}

function goToNext(container) {

    const btn = container.querySelector('.nav-btn-prev');
    const btnWidth = btn.getBoundingClientRect().right;

    const contentArea = container.querySelector('.swiper-content')
    const items = contentArea.querySelectorAll('.swiper-item');

    for (let i = 0; i < items.length; i++) {
        const start = Math.floor(
            items[i].getBoundingClientRect().left
        );
        if (start - btnWidth > 5) {
            contentArea.scrollLeft += start - btnWidth;
            break;
        };
    }
}

function goToPrev(container) {

    const btn = container.querySelector('.nav-btn-prev');
    const btnWidth = btn.getBoundingClientRect().right;

    const contentArea = container.querySelector('.swiper-content')
    const items = contentArea.querySelectorAll('.swiper-item');

    for (let i = items.length - 1; i >= 0; i--) {

        const start = Math.ceil(
            items[i].getBoundingClientRect().left
        );

        if (start < (btnWidth - 5)) {
            contentArea.scrollLeft += start - btnWidth;
            break;
        }
    };    
}