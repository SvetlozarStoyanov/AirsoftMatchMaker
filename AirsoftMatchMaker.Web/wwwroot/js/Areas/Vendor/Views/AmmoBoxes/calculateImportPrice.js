const pricePerUnit = document.getElementById('price');
const addedQuantity = document.getElementById('addedQuantity');
const finalImportPriceForSubmit = document.getElementById('finalImportPriceForSubmit');
const finalImportPriceForUi = document.getElementById('finalImportPriceForUi');

const submitFormBtn = document.getElementById('submitFormBtn');
const userCredits = parseFloat(document.getElementById('userCredits').value);


calculatePrice();
UpdatePricePerUnit();
function UpdatePricePerUnit() {
    var NameValue = pricePerUnit.value;
    // use it
    console.log(NameValue);
    calculatePrice();
}
pricePerUnit.onchange = UpdatePricePerUnit;
pricePerUnit.onblur = UpdatePricePerUnit;

function UpdateAddedQuantity() {
    var NameValue = addedQuantity.value;
    // use it
    console.log(NameValue);
    calculatePrice();
}
addedQuantity.onchange = UpdateAddedQuantity;
addedQuantity.onblur = UpdateAddedQuantity;

function updateSubmitFormBtnStatus() {
    if (parseFloat(finalImportPriceForSubmit.value) > userCredits) {
        submitFormBtn.disabled = true;
    } else {
        submitFormBtn.disabled = false;
    }
}


function calculatePrice() {
    console.log(pricePerUnit.value);
    console.log(addedQuantity.value);

    finalImportPriceForSubmit.value = (parseInt(addedQuantity.value) * parseFloat(pricePerUnit.value) / 2);
    finalImportPriceForUi.value = finalImportPriceForSubmit.value;
    console.log(finalImportPriceForUi.value);
    updateSubmitFormBtnStatus();
    console.log(`Final import price: ${finalImportPriceForSubmit.value} credits.`);
}