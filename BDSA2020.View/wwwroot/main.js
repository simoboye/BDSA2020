function goPrev() {
  document.getElementById('scroll-container').scrollBy({ 
    left: -40,
    behavior: 'smooth' 
  });
}

function goNext() {
  document.getElementById('scroll-container').scrollBy({ 
    left: 40,
    behavior: 'smooth' 
  });
}

function checkMore(){
    //calculates if there is more desriptions from scroll values (455 is height of one description (px))
    if((document.getElementById('scroll-container').scrollHeight - 455.0) == document.getElementById('scroll-container').scrollTop){
      console.log("NO MORE DESCRIPTION")
      //TODO add loadMore() logic here
    }
}

function showAlert(){
  document.getElementById('savedAlert').hidden = false;
  setTimeout(function(){ hideAlert() }, 3000);
}

function hideAlert(){
  document.getElementById('savedAlert').hidden = true;
}