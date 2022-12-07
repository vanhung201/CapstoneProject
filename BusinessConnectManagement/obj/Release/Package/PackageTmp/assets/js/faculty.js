let formAdd = document.getElementById("addForm");
let formDelete = document.getElementById("deleteForm");
let formUpdate = document.getElementById("updateForm");

document.querySelector(".btn--delete").addEventListener("click", function () {
  formDelete.classList.add("is-appear");
});

document
  .querySelector(".form#deleteForm .form-icon")
  .addEventListener("click", function () {
    formDelete.classList.remove("is-appear");
  });

document.querySelector(".btn--update").addEventListener("click", function () {
  formUpdate.classList.add("is-appear");
});

document
  .querySelector(".form#updateForm .form-icon")
  .addEventListener("click", function () {
    formUpdate.classList.remove("is-appear");
  });

document.querySelector(".btn--create").addEventListener("click", function () {
  formAdd.classList.add("is-appear");
});

document
  .querySelector(".form#addForm .form-icon")
  .addEventListener("click", function () {
    formAdd.classList.remove("is-appear");
  });
