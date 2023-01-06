let form = document.querySelector(".form");
let icon = document.querySelector(".form-icon");
let isHide = "is-hide";
let isAppear = "is-appear";
let inputPass = document.getElementById("pass");
let inputFail = document.getElementById("fail");

let inputFinish = document.getElementById("finish");
let inputUnfinish = document.getElementById("unfinish");

icon.addEventListener("click", function () {
  form.classList.remove(isAppear);
});
let buttonUpdate = document.querySelectorAll(".btn--update");

for (let i = 0; i < buttonUpdate.length; i++) {
  buttonUpdate[i].onclick = function () {
    form.classList.add(isAppear);

    let result = document.querySelectorAll("td:nth-child(5)")[i].textContent;
    switch (result) {
      case "Đậu":
        inputPass.setAttribute("checked", "checked");
        break;
      case "Hoàn thành":
        inputFinish.setAttribute("checked", "checked");
        break;
      case "Rớt":
        inputFail.setAttribute("checked", "checked");
        break;
      case "Chưa hoàn thành":
        inputUnfinish.setAttribute("checked", "checked");
        break;
    }

    document.getElementById("comment").textContent =
      document.querySelectorAll("td:nth-child(4)")[i].textContent;
    };
   
}
