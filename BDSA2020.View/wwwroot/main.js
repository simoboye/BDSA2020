window.CardAnimation = () => {

};

function goPrev() {
  console.log("up");
  document.getElementById('scroll-container').scrollBy({ 
    top: -40,
    behavior: 'smooth' 
  });
}

function goNext() {
  console.log("down");
  document.getElementById('scroll-container').scrollBy({ 
    top: 40,
    behavior: 'smooth' 
  });
}