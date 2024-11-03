// Define colors array at the top level
window.logoColors = [
    "#98FB98", // Pale Green
    "#FFD700", // Gold
    "#FFA500", // Orange
    "#32CD32", // Lime Green
    "#87CEEB", // Sky Blue
    "#4169E1", // Royal Blue
    "#800080", // Purple
    "#FF69B4", // Hot Pink
    "#FF0000", // Red
    "#000000", // Black
];

document.addEventListener("DOMContentLoaded", function () {
    // SVG elements
    const book = document.getElementById("book");
    const bookmark = document.getElementById("bookmark");
    const background = document.getElementById("background");
    const pages = document.getElementById("pages").getElementsByTagName("line");
    const face = document.getElementById("face").children;

    // Accessory elements
    const glasses = document.getElementById("glasses");
    const partyHat = document.getElementById("party-hat");
    const headphones = document.getElementById("headphones");
    const magicWand = document.getElementById("magic-wand");

    // Default values for reset
    const defaults = {
        bookColor: "#0077cc",
        bookmarkColor: "#ffcc00",
        bgColor: "#ffffff",
        glasses: false,
        partyHat: false,
        headphones: false,
        magicWand: false,
    };

    // Initialize color buttons
    document.querySelectorAll(".color-btn").forEach((btn) => {
        btn.style.backgroundColor = btn.getAttribute("data-color");
    });

    // Element select and color buttons
    const elementSelect = document.getElementById("elementSelect");
    document.querySelectorAll("#colorPalette .color-btn").forEach((btn) => {
        btn.addEventListener("click", function () {
            const selectedElement = elementSelect.value;
            const color = this.getAttribute("data-color");

            // Remove active state from all buttons
            document
                .querySelectorAll("#colorPalette .color-btn")
                .forEach((b) => b.classList.remove("active"));

            // Add active state to clicked button
            this.classList.add("active");

            // Update the selected element's color
            switch (selectedElement) {
                case "book":
                    updateBookColor(color);
                    break;
                case "bookmark":
                    updateBookmarkColor(color);
                    break;
                case "background":
                    updateBackgroundColor(color);
                    break;
            }
        });
    });

    // Update functions
    function updateBookColor(color) {
        if (book) {
            book.setAttribute("fill", color);
            const pagesColor = color === "#ffffff" ? "#000000" : "#ffffff";
            Array.from(pages).forEach((line) =>
                line.setAttribute("stroke", pagesColor)
            );
        }
    }

    function updateBookmarkColor(color) {
        if (bookmark) bookmark.setAttribute("fill", color);
    }

    function updateBackgroundColor(color) {
        if (background) background.setAttribute("fill", color);
    }

    // Toggle accessories
    function toggleAccessory(accessory, show) {
        if (accessory) {
            accessory.style.display = show ? "block" : "none";
        }
    }

    // Reset function
    function resetCustomization() {
        // Reset colors to defaults
        updateBookColor(defaults.bookColor);
        updateBookmarkColor(defaults.bookmarkColor);
        updateBackgroundColor(defaults.bgColor);

        // Remove all active states
        document.querySelectorAll(".color-btn").forEach((btn) => {
            btn.classList.remove("active");
        });

        // Reset accessories
        const accessories = [glasses, partyHat, headphones, magicWand];
        const toggles = [
            glassesToggle,
            partyHatToggle,
            headphonesToggle,
            magicWandToggle,
        ];

        accessories.forEach((accessory, index) => {
            if (toggles[index]) {
                toggles[index].checked = false;
                toggleAccessory(accessory, false);
            }
        });
    }

    // Update randomize function to use logoColors array
    function randomizeAll() {
        // Remove active state from all buttons
        document.querySelectorAll(".color-btn").forEach((btn) => {
            btn.classList.remove("active");
        });

        // Randomize colors using the shared color array
        updateBookColor(logoColors[Math.floor(Math.random() * logoColors.length)]);
        updateBookmarkColor(
            logoColors[Math.floor(Math.random() * logoColors.length)]
        );
        updateBackgroundColor(
            logoColors[Math.floor(Math.random() * logoColors.length)]
        );

        // Randomize accessories
        const accessories = [glasses, partyHat, headphones, magicWand];
        const toggles = [
            glassesToggle,
            partyHatToggle,
            headphonesToggle,
            magicWandToggle,
        ];

        accessories.forEach((accessory, index) => {
            if (toggles[index]) {
                const show = Math.random() > 0.5;
                toggles[index].checked = show;
                toggleAccessory(accessory, show);
            }
        });
    }

    // Save logo function
    function saveLogo() {
        const svg = document.getElementById("logo");
        if (!svg) return;

        const serializer = new XMLSerializer();
        const svgStr = serializer.serializeToString(svg);

        const canvas = document.createElement("canvas");
        const ctx = canvas.getContext("2d");
        const img = new Image();

        canvas.width = svg.width.baseVal.value;
        canvas.height = svg.height.baseVal.value;

        img.onload = function () {
            ctx.drawImage(img, 0, 0);
            const a = document.createElement("a");
            a.href = canvas.toDataURL("image/png");
            a.download = "aspharmony-logo.png";
            a.click();
        };

        img.src =
            "data:image/svg+xml;base64," + btoa(unescape(encodeURIComponent(svgStr)));
    }

    // Fix accessory toggle variables - update IDs to match HTML
    const glassesToggle = document.getElementById("glasses-toggle");
    const partyHatToggle = document.getElementById("party-hat-toggle");
    const headphonesToggle = document.getElementById("headphones-toggle");
    const magicWandToggle = document.getElementById("magic-wand-toggle");

    // Add event listeners for accessories
    if (glassesToggle) {
        glassesToggle.addEventListener("change", (e) =>
            toggleAccessory(glasses, e.target.checked)
        );
    }
    if (partyHatToggle) {
        partyHatToggle.addEventListener("change", (e) =>
            toggleAccessory(partyHat, e.target.checked)
        );
    }
    if (headphonesToggle) {
        headphonesToggle.addEventListener("change", (e) =>
            toggleAccessory(headphones, e.target.checked)
        );
    }
    if (magicWandToggle) {
        magicWandToggle.addEventListener("change", (e) =>
            toggleAccessory(magicWand, e.target.checked)
        );
    }

    // Remove duplicate randomizeButton declaration
    const randomizeBtn = document.getElementById("randomizeAll");
    if (randomizeBtn) {
        randomizeBtn.addEventListener("click", randomizeAll);
    }

    const clearButton = document.getElementById("clearAll");
    if (clearButton) {
        clearButton.addEventListener("click", resetCustomization);
    }

    const saveButton = document.getElementById("saveLogo");
    if (saveButton) {
        saveButton.addEventListener("click", saveLogo);
    }
});
