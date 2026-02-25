window.addEventListener('load', function () {
    // Wait for Swagger UI to fully render
    function injectFooter() {
        var existing = document.getElementById('custom-footer');
        if (existing) return;

        var footer = document.createElement('div');
        footer.id = 'custom-footer';
        footer.style.cssText = `
            text-align: center;
            padding: 15px;
            color: #ffffff;
            font-size: 14px;
            font-family: Arial, sans-serif;
            background-color: #1a1a2e;
            letter-spacing: 1px;
            margin-top: 30px;
            position: fixed;
            bottom: 0;
            width: 100%;
            z-index: 9999;
        `;
        footer.innerHTML = 'Developed by <strong>Lokpavan P</strong>';
        document.body.appendChild(footer);
    }

    // Keep trying until Swagger UI is ready
    var interval = setInterval(function () {
        if (document.querySelector('.swagger-ui')) {
            injectFooter();
            clearInterval(interval);
        }
    }, 500);
});