export function playWinnerEffect(effectIndex) {
    const prefersReducedMotion = window.matchMedia('(prefers-reduced-motion: reduce)').matches;

    if (prefersReducedMotion) {
        showStaticToast();
        return;
    }

    switch (effectIndex) {
        case 0:
            confettiBurst();
            break;
        case 1:
            goldStarShower();
            break;
        case 2:
            spotlightSweep();
            break;
        case 3:
            applauseText();
            break;
        case 4:
            hollywoodSignFlash();
            break;
        case 5:
            redCarpetRoll();
            break;
        case 6:
            filmReelSpin();
            break;
        default:
            confettiBurst();
    }
}

function showStaticToast() {
    const toast = document.createElement('div');
    toast.textContent = '? Correct!';
    toast.style.cssText = `
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        background: #28a745;
        color: white;
        padding: 2rem 3rem;
        border-radius: 10px;
        font-size: 2rem;
        font-weight: bold;
        z-index: 10000;
    `;
    document.body.appendChild(toast);
    setTimeout(() => toast.remove(), 2000);
}

function confettiBurst() {
    const colors = ['#ff0000', '#00ff00', '#0000ff', '#ffff00', '#ff00ff', '#00ffff'];
    const container = document.createElement('div');
    container.style.cssText = 'position:fixed;top:0;left:0;width:100%;height:100%;pointer-events:none;z-index:9999;';

    for (let i = 0; i < 50; i++) {
        const confetti = document.createElement('div');
        confetti.style.cssText = `
            position: absolute;
            width: 10px;
            height: 10px;
            background: ${colors[Math.floor(Math.random() * colors.length)]};
            top: -10px;
            left: ${Math.random() * 100}%;
            animation: confettiFall ${2 + Math.random() * 2}s linear forwards;
        `;
        container.appendChild(confetti);
    }

    const style = document.createElement('style');
    style.textContent = `
        @keyframes confettiFall {
            to {
                transform: translateY(100vh) rotate(${Math.random() * 360}deg);
                opacity: 0;
            }
        }
    `;
    document.head.appendChild(style);
    document.body.appendChild(container);

    setTimeout(() => {
        container.remove();
        style.remove();
    }, 4000);
}

function goldStarShower() {
    const container = document.createElement('div');
    container.style.cssText = 'position:fixed;top:0;left:0;width:100%;height:100%;pointer-events:none;z-index:9999;';

    for (let i = 0; i < 30; i++) {
        const star = document.createElement('div');
        star.textContent = '?';
        star.style.cssText = `
            position: absolute;
            color: gold;
            font-size: ${20 + Math.random() * 30}px;
            top: -50px;
            left: ${Math.random() * 100}%;
            animation: starFall ${3 + Math.random() * 2}s linear forwards;
        `;
        container.appendChild(star);
    }

    const style = document.createElement('style');
    style.textContent = `
        @keyframes starFall {
            to {
                transform: translateY(100vh) rotate(360deg);
                opacity: 0;
            }
        }
    `;
    document.head.appendChild(style);
    document.body.appendChild(container);

    setTimeout(() => {
        container.remove();
        style.remove();
    }, 5000);
}

function spotlightSweep() {
    const spotlight = document.createElement('div');
    spotlight.style.cssText = `
        position: fixed;
        top: 0;
        left: -100%;
        width: 100%;
        height: 100%;
        background: radial-gradient(circle at center, rgba(255,255,255,0.3) 0%, transparent 70%);
        pointer-events: none;
        z-index: 9999;
        animation: spotlightMove 2s ease-in-out forwards;
    `;

    const style = document.createElement('style');
    style.textContent = `
        @keyframes spotlightMove {
            to {
                left: 100%;
                opacity: 0;
            }
        }
    `;
    document.head.appendChild(style);
    document.body.appendChild(spotlight);

    setTimeout(() => {
        spotlight.remove();
        style.remove();
    }, 2000);
}

function applauseText() {
    const overlay = document.createElement('div');
    overlay.textContent = '?? BRAVO! ??';
    overlay.style.cssText = `
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%) scale(0);
        font-size: 4rem;
        font-weight: bold;
        color: gold;
        text-shadow: 2px 2px 4px rgba(0,0,0,0.8);
        pointer-events: none;
        z-index: 9999;
        animation: scaleIn 2s ease-in-out forwards;
    `;

    const style = document.createElement('style');
    style.textContent = `
        @keyframes scaleIn {
            0% { transform: translate(-50%, -50%) scale(0); opacity: 0; }
            10% { transform: translate(-50%, -50%) scale(1.2); opacity: 1; }
            90% { transform: translate(-50%, -50%) scale(1); opacity: 1; }
            100% { transform: translate(-50%, -50%) scale(1); opacity: 0; }
        }
    `;
    document.head.appendChild(style);
    document.body.appendChild(overlay);

    setTimeout(() => {
        overlay.remove();
        style.remove();
    }, 2000);
}

function hollywoodSignFlash() {
    const flash = document.createElement('div');
    flash.style.cssText = `
        position: fixed;
        top: 0;
        left: 0;
        width: 100%;
        height: 100%;
        background: white;
        pointer-events: none;
        z-index: 9998;
        animation: flashPulse 1s ease-out forwards;
    `;

    const text = document.createElement('div');
    text.textContent = '? YOU\'RE A STAR ?';
    text.style.cssText = `
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%);
        font-size: 3rem;
        font-weight: bold;
        color: gold;
        text-shadow: 3px 3px 6px rgba(0,0,0,0.9);
        pointer-events: none;
        z-index: 9999;
        animation: textFlash 1s ease-out forwards;
    `;

    const style = document.createElement('style');
    style.textContent = `
        @keyframes flashPulse {
            0% { opacity: 0; }
            10% { opacity: 1; }
            100% { opacity: 0; }
        }
        @keyframes textFlash {
            0% { opacity: 0; transform: translate(-50%, -50%) scale(0.5); }
            50% { opacity: 1; transform: translate(-50%, -50%) scale(1.1); }
            100% { opacity: 0; transform: translate(-50%, -50%) scale(1); }
        }
    `;
    document.head.appendChild(style);
    document.body.appendChild(flash);
    document.body.appendChild(text);

    setTimeout(() => {
        flash.remove();
        text.remove();
        style.remove();
    }, 1000);
}

function redCarpetRoll() {
    const carpet = document.createElement('div');
    carpet.style.cssText = `
        position: fixed;
        bottom: -100%;
        left: 0;
        width: 100%;
        height: 100px;
        background: linear-gradient(to right, #8B0000, #DC143C, #8B0000);
        pointer-events: none;
        z-index: 9999;
        animation: carpetRoll 3s ease-in-out forwards;
    `;

    const style = document.createElement('style');
    style.textContent = `
        @keyframes carpetRoll {
            0% { bottom: -100%; }
            30% { bottom: 0; }
            70% { bottom: 0; }
            100% { bottom: -100%; }
        }
    `;
    document.head.appendChild(style);
    document.body.appendChild(carpet);

    setTimeout(() => {
        carpet.remove();
        style.remove();
    }, 3000);
}

function filmReelSpin() {
    const reel = document.createElement('div');
    reel.textContent = '??';
    reel.style.cssText = `
        position: fixed;
        top: 50%;
        left: 50%;
        transform: translate(-50%, -50%) scale(0);
        font-size: 10rem;
        pointer-events: none;
        z-index: 9999;
        animation: reelSpin 2s ease-in-out forwards;
    `;

    const style = document.createElement('style');
    style.textContent = `
        @keyframes reelSpin {
            0% { transform: translate(-50%, -50%) scale(0) rotate(0deg); opacity: 0; }
            50% { transform: translate(-50%, -50%) scale(1) rotate(360deg); opacity: 1; }
            100% { transform: translate(-50%, -50%) scale(0) rotate(720deg); opacity: 0; }
        }
    `;
    document.head.appendChild(style);
    document.body.appendChild(reel);

    setTimeout(() => {
        reel.remove();
        style.remove();
    }, 2000);
}
