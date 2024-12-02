// Generate Particles Dynamically
const particleBackground = document.querySelector('.particle-background');

function createParticle() {
    const particle = document.createElement('div');
    particle.classList.add('particle');

    const size = Math.random() * 4 + 1; // Random size between 1px and 5px
    particle.style.width = `${size}px`;
    particle.style.height = `${size}px`;

    particle.style.left = `${Math.random() * 100}vw`; // Random horizontal position
    particle.style.top = `${Math.random() * 100}vh`; // Random vertical position

    particle.style.animationDuration = `${Math.random() * 3 + 2}s`; // Random animation duration
    particle.style.animationDelay = `${Math.random() * 2}s`; // Random animation delay

    particleBackground.appendChild(particle);

    // Remove particle after animation
    setTimeout(() => {
        particle.remove();
    }, 5000);
}

// Continuously Create Particles
setInterval(createParticle, 200);

document.onkeydown = function (e) {
    if (e.ctrlKey && e.shiftKey && e.keyCode == 'I'.charCodeAt(0) || e.ctrlKey && e.keyCode == 'U'.charCodeAt(0)) {
        return false;
    } else {
        return true;
    }
};