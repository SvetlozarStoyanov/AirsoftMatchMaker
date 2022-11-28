const pricePerUnit = document.getElementById('price');
const finalImportPriceForSubmit = document.getElementById('finalImportPriceForSubmit');
const finalImportPriceForUI = document.getElementById('finalImportPriceForUI');

calculatePrice();

function UpdatePricePerUnit() {
    var NameValue = pricePerUnit.value;
    console.log(NameValue);
    calculatePrice();
}

pricePerUnit.onchange = UpdatePricePerUnit;
pricePerUnit.onblur = UpdatePricePerUnit;


function calculatePrice() {
    console.log(pricePerUnit.value);
    finalImportPriceForSubmit.value =  parseFloat(pricePerUnit.value) / 2;
    finalImportPriceForUI.value = finalImportPriceForSubmit.value;
    console.log(`Final import price: ${finalImportPriceForSubmit.value} credits.`);
}


