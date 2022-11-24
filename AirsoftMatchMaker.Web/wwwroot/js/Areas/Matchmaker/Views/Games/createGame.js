const dateLabel = document.getElementById('dateLabel');
const inputForModel = document.getElementById('inputForModel');

window.addEventListener('onload', function (e) {
    inputForModel.value = dateLabel.textContent;
    console.log(inputForModel.value);
});