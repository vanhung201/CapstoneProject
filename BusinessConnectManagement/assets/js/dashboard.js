let typeNumber = document.getElementById("typeChart");
let peopleNumber = document.getElementById("peopleChart");

let numBlock = 1;
let numActive = 6;
let total = numBlock + numActive;

let numStudent = 5;
let numMentor = 1;
let numFaculty = 1;
let block = (numBlock * 100) / total;
let active = (numActive * 100) / total;
let student = (numStudent * 100) / total;
let mentor = (numMentor * 100) / total;
let faculty = (numFaculty * 100) / total;

if (peopleNumber != undefined && typeNumber != undefined) {
  peopleNumber.style.setProperty(
    "background-image",
    `repeating-conic-gradient(from 0deg,
          #f64f59 0deg ${block * 3.6 + "deg"},
          #00b09b ${block * 3.6 + "deg"} 360deg)`
  );

  typeNumber.style.setProperty(
    "background-image",
    `repeating-conic-gradient(from 0deg,
          #2980B9 0deg ${student * 3.6 + "deg"},
          #4e54c8 ${student * 3.6 + "deg"} calc(${student * 3.6 + "deg"} + ${
      mentor * 3.6 + "deg"
    }) ,
          #CAC531 calc(${student * 3.6 + "deg"} + ${
      mentor * 3.6 + "deg"
    }) 360deg`
  );

  const blockCount = document.getElementById("block");

  const activeCount = document.getElementById("active");

  const studentCount = document.getElementById("student");

  const facultyCount = document.getElementById("faculty");
  const mentorCount = document.getElementById("mentor");

  blockCount.innerHTML = `${numBlock} người bị khóa = ${block.toFixed()}%`;

  activeCount.innerHTML = `${numActive} người hoạt động = ${active.toFixed()}%`;

  studentCount.innerHTML = `${numStudent} Sinh viên = ${student.toFixed()}%`;

  facultyCount.innerHTML = `${numFaculty} Ban chủ nhiệm Khoa = ${faculty.toFixed()}%`;
  mentorCount.innerHTML = `${numMentor} Giảng viên hướng dẫn = ${mentor.toFixed()}%`;
}
