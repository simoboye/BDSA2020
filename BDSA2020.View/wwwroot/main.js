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
