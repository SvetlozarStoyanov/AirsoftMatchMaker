const pricePerUnit = document.getElementById('price');
const addedQuantity = document.getElementById('addedQuantity');
const finalImportPriceForSubmit = document.getElementById('finalImportPriceForSubmit');
const finalImportPriceForUI = document.getElementById('finalImportPriceForUI');

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

function calculatePrice() {
    console.log(pricePerUnit.value);
    console.log(addedQuantity.value);
    finalImportPriceForSubmit.value = (parseInt(addedQuantity.value) * parseFloat(pricePerUnit.value) / 2);
    finalImportPriceForUI.value = finalImportPriceForSubmit.value;
    console.log(`Final import price: ${finalImportPriceForSubmit.value} credits.`);
}


