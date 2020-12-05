
window.CardAnimation = () => {
  //use this for future JS where Blazor needs to be loaded
  //document.getElementById('scroll-container').onscroll = function() {
  //    checkMore();
  //};
}

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

document.querySelector('#controls').addEventListener('click', (event) => {
  const $slide = document.querySelector(event.target.getAttribute('href'));
  if (!$slide) return;
  
  if ($slide.scrollIntoViewIfNeeded) {
    event.preventDefault();
    $slide.scrollIntoViewIfNeeded();
  } else if ($slide.scrollIntoView) {
    event.preventDefault();
    $slide.scrollIntoView();
  }
});


function checkMore(){
    //calculates if there is more desriptions from scroll values (455 is height of one description (px))
    console.log((document.getElementById('scroll-container').scrollLeft - document.getElementById('scroll-container').clientWidth));
    console.log(document.getElementById('scroll-container').scrollWidth);
    if((document.getElementById('scroll-container').scrollLeft - document.getElementById('scroll-container').clientWidth) == document.getElementById('scroll-container').scrollLeft){
      console.log("NO MORE DESCRIPTION")
      //TODO add loadMore() logic here
    }
  }
