
window.CardAnimation = () => {
  //use this for future JS where Blazor needs to be loaded
  document.getElementById('scroll-container').onscroll = function() {
      checkMore();
  };
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
 // checkMore();
}




function checkMore(){
    //calculates if there is more desriptions from scroll values (455 is height of one description (px))
    if((document.getElementById('scroll-container').scrollHeight - 455.0) == document.getElementById('scroll-container').scrollTop){
      console.log("NO MORE DESCRIPTION")
      //TODO add loadMore() logic here
    }

}