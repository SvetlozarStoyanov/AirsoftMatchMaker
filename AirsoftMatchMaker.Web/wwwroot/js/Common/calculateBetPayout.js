const teamRedId = document.getElementById('teamRedId');
const teamBlueId = document.getElementById('teamBlueId');
const chosenTeamId = document.getElementById('chosenTeamId');
const teamRedOdds = parseFloat(document.getElementById('teamRedOdds').value);
const teamBlueOdds = parseFloat(document.getElementById('teamBlueOdds').value);
const creditsPlaced = document.getElementById('creditsPlaced');
const payout = document.getElementById('payout');

console.log('test');


updatePayout();

function updateCreditsPlaced() {

    updatePayout();
}
creditsPlaced.onchange = updateCreditsPlaced;
creditsPlaced.onblur = updateCreditsPlaced;

function updateChosenTeamId() {

    updatePayout();
}

chosenTeamId.onchange = updateChosenTeamId;
chosenTeamId.onblur = updateChosenTeamId;




function updatePayout() {
    if (chosenTeamId.value == teamRedId.value) {
        payout.value = calculatePayout(teamRedOdds);
    } else if (chosenTeamId.value == teamBlueId.value) {
        payout.value = calculatePayout(teamBlueOdds);
    }
}

function calculatePayout(odds) {
    var profit = parseFloat(creditsPlaced.value);
    if (odds < 0) {
        profit += profit * (100 / Math.abs(odds));
    }
    else {
        profit += profit * (odds / 100);
    }
    console.log(profit.toFixed(2));
    return profit.toFixed(2);
}
