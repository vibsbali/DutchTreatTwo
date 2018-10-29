$(document).ready(function() {
   var x = 0;
   var s = "";

   console.log("Hello Pluralsight");

   var button = $("#buyButton");
   button.on("click",
      function() {
         console.log("Buying Item!");
      });

   var productInfo = $(".product-info li");
   productInfo.click("click",
      function() {
         console.log("You clicked on " + $(this).text());
      });

});

