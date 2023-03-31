// MenuIcon -> arrow
let menu = document.querySelector(".menu");
let closeBtn = document.querySelector("#menu-icon");

closeBtn.addEventListener("click", () => {
    menu.classList.toggle("menu--close");
    menuIconChange();
});

function menuIconChange() {
    if (menu.classList.contains("menu--close")) {
        closeBtn.classList.replace("fa-long-arrow-left", "fa-long-arrow-right");
    } else {
        closeBtn.classList.replace("fa-long-arrow-right", "fa-long-arrow-left");
    }
}

// Icon Bars
const bars = document.getElementById("icon");
let content = document.querySelector(".content");


bars.addEventListener("click", function () {
    if (menu.style.display === "none") {
        content.classList.remove("content--100");
        menu.style.display = "block";
        menu.style.animation = "growIn 0.15s linear 0s 1 alternate forwards";
        content.classList.add("content--max");
    } else {
        content.classList.remove("content--max");
        menu.style.display = "none";
    content.classList.add("content--100");
    }
});

// Responsive
let x = window.matchMedia("screen and (min-width: 1024px)");
function myFunction() {
    if (x.matches) {
        //menu.classList.toggle("menu--close");
        document.querySelector("body").classList.remove("is-hide");
    } else {
        //menu.classList.remove("menu--close");
        document.querySelector("body").classList.add("is-hide");
    }
}
myFunction(x);
x.addListener(myFunction);

// MoveTop
const moveTop = document.querySelector("#moveTop");

window.addEventListener("scroll", () => {
    const scrolled = window.scrollY;
    if (scrolled >= 20) {
        moveTop.style.opacity = "1";
        moveTop.style.visibility = "visible";
    } else {
        moveTop.style.opacity = "0";
        moveTop.style.visibility = "hidden";
    }
});

// Greet
const greet = [
    "Chúc bạn một ngày làm việc tốt lành",
    "Chúc bạn có một trải nghiệm thú vị",
    "Chúc bạn luôn vui vẻ và bình an",
    "Hãy luôn vui tươi nhé!",
];

let wishElement = document.querySelector(".greeting-wish");
if (wishElement !== null) {
    wishElement.textContent = greet[Math.floor(Math.random() * greet.length)];

    // Time
    const timeElement = document.querySelector(".greeting-time");
    const dateElement = document.querySelector(".greeting-date");

    function formatTime(date) {
        const hours = date.getHours();
        const minutes = date.getMinutes();
        const seconds = date.getSeconds();
        return `${hours.toString().padStart(2, "0")}:${minutes
            .toString()
            .padStart(2, "0")}:${seconds.toString().padStart(2, "0")}`;
    }

    function formatDate(date) {
        const DAYS = [
            "Chủ nhật",
            "Thứ hai",
            "Thứ ba",
            "Thứ tư",
            "Thứ năm",
            "Thứ sáu",
            "Thứ bảy",
        ];

        return `${DAYS[date.getDay()]}, Ngày ${date.getDate()} Tháng ${date.getMonth() + 1
            } ${date.getFullYear()}`;
    }

    setInterval(() => {
        const now = new Date();
        timeElement.textContent = formatTime(now);
        dateElement.textContent = formatDate(now);
    }, 100);
}
