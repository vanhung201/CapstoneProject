let option = document.querySelector(".option");
let login = document.querySelector(".login");
let optionItem = document.querySelector(".option-item:last-child .option-link");
let isHide = "is-hide";
let isAppear = "is-appear";
let loginIcon = document.querySelector(".login-icon");

optionItem.addEventListener("click", function () {
  option.classList.add(isHide);

  setTimeout(function () {
    login.classList.add(isAppear);
  }, 100);
});

loginIcon.addEventListener("click", function () {
  login.classList.remove(isAppear);

  setTimeout(function () {
    option.classList.remove(isHide);
  }, 100);
});
