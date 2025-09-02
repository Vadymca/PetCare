// counter-animation.ts
export function animateCounter(
  element: HTMLElement,
  targetValue: number,
  duration = 2000
) {
  let startValue = Math.floor(targetValue * 0.9);
  const stepTime = Math.abs(Math.floor(duration / targetValue));
  const increment = targetValue > 0 ? 1 : -1;

  const updateCounter = () => {
    startValue += increment;
    element.textContent = Math.round(startValue).toString();

    if (startValue < targetValue) {
      setTimeout(updateCounter, stepTime);
    } else {
      element.textContent = targetValue.toString();
      element.classList.add('counter-animated');
    }
  };

  updateCounter();
}
