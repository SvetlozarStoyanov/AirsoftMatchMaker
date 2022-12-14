const collapseBtn = document.getElementById('collapseBtn');
const collapseBtnIcon = document.getElementById('collapseBtnIcon');
collapseBtn.addEventListener('click', function () {
    console.log("Button clicked!");
    if (collapseBtnIcon.classList.contains('bi-arrow-up')) {
        collapseBtnIcon.classList.remove('bi-arrow-up');
        collapseBtnIcon.classList.add('bi-arrow-down');
    }
    else if (collapseBtnIcon.classList.contains('bi-arrow-down')) {
        collapseBtnIcon.classList.remove('bi-arrow-down');
        collapseBtnIcon.classList.add('bi-arrow-up');
    }
})