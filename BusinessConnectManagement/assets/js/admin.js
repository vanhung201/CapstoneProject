let form = document.querySelector(".form");
let icon = document.querySelector(".form-icon");
let isHide = "is-hide";
let isAppear = "is-appear";
let inputEmail = document.getElementById("email");
let inputRole = document.getElementById("role");

let inputActive = document.getElementById("active");
let inputBlock = document.getElementById("block");

icon.addEventListener("click", function () {
  form.classList.remove(isAppear);
});
let buttonUpdate = document.querySelectorAll(".btn--update");
for (let i = 0; i < buttonUpdate.length; i++) {
  buttonUpdate[i].onclick = function () {
    form.classList.add(isAppear);
    document.getElementById("email").value =
      document.querySelectorAll("td:nth-child(1)")[i].textContent;
    document.getElementById("role").value =
      document.querySelectorAll("td:nth-child(4)")[i].textContent;

    let result = document.querySelectorAll("td:nth-child(5)")[i].textContent;
    if (result == "Đang hoạt động") {
      inputActive.setAttribute("checked", "checked");
    } else if (result == "Bị khóa") {
      inputBlock.setAttribute("checked", "checked");
    }
  };
}
