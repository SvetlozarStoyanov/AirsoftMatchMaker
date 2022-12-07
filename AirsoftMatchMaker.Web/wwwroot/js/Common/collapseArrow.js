const collapseBtn = document.getElementById('collapseBtn');
const collapseBtnText = document.getElementById('collapseBtnText');
collapseBtn.addEventListener('click', function () {
    console.log("Button clicked!");
    collapseBtnText.textContent = collapseBtnText.textContent == 'Characteristics +' ? 'Characteristics -' : 'Characteristics +';
})