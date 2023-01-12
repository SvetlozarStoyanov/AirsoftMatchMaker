const teamRedPoints = document.getElementById('teamRedPoints');
const teamBluePoints = document.getElementById('teamBluePoints');
const maxPoints = parseInt(document.getElementById('maxPoints').value);
const submitFormBtn = document.getElementById('submitFormBtn');
const errorBox = document.getElementById('errorBox');
const errorMessage = document.getElementById('errorMessage');
console.log('test');
console.log(maxPoints);

updateSubmitFormBtnStatus();

function updateTeamRedPoints() {
    updateSubmitFormBtnStatus();
}

teamRedPoints.onchange = updateTeamRedPoints;
teamRedPoints.onblur = updateTeamRedPoints;


function updateTeamBluePoints() {
    updateSubmitFormBtnStatus();
}

teamBluePoints.onchange = updateTeamBluePoints;
teamBluePoints.onblur = updateTeamBluePoints;

function updateSubmitFormBtnStatus() {
    var redPoints = parseInt(teamRedPoints.value);
    var bluePoints = parseInt(teamBluePoints.value);
    submitFormBtn.disabled = true;
    errorBox.classList.remove('d-none');
    errorMessage.textContent = '';

    if (redPoints < 0 || bluePoints < 0) {
        errorMessage.textContent = `Points cannot be below 0 points!`;
    }
    else if (redPoints > maxPoints) {
        errorMessage.textContent = `Red team points cannot be above ${maxPoints} points!`;
    }
    else if (bluePoints > maxPoints) {
        errorMessage.textContent = `Blue team points cannot be above ${maxPoints} points!`;
    }
    else if (redPoints === 0 && bluePoints === 0) {

    }
    else if (redPoints === bluePoints) {
        errorMessage.textContent = `Points cannot be equal!`;
    }
    else {
        submitFormBtn.disabled = false;
        errorBox.classList.add('d-none');
    }
}