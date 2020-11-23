$(document).ready(function () {
    var x = 10_000_000;
    var alertGreeting = "hello";

    console.log(`${alertGreeting} you are the ${x}th visitor`);

    var button = $("buyButton");

    button.on("click", function () {
        console.log("Buying Item");
    });

// what happens when we have a bunch of stuff of different types from different parts of the page here?
    var productInfo = $(".product-props");

    productInfo.on("click", function () {
        console.log(`You clicked on ${$(this).text()}`);
    });
    
    var $loginToggle = $("#loginToggle");
    var $popupForm = $(".popupForm");
    $popupForm.hide();
    
    $loginToggle.on("click", function(){
        $popupForm.slideToggle(100);
    })
});