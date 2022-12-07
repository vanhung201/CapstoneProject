const greet = [
  "Chúc bạn một ngày làm việc tốt lành",
  "Chúc bạn có một trải nghiệm thú vị",
  "Chúc bạn luôn vui vẻ và bình an",
  "Hãy luôn vui tươi nhé!",
];

let wishElement = document.querySelector(".greeting-wish");
wishElement.textContent = greet[Math.floor(Math.random() * greet.length)];
