const userCredits = parseFloat(document.getElementById('userCredits').value);
const buyBtn = document.getElementById('buyBtn');
const price = parseFloat(document.getElementById('price').value);

if (price > userCredits) {
    buyBtn.disabled = true;
} else {
    buyBtn.disabled = false;
}
console.log(userCredits)