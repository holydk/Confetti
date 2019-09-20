import { NguCarouselConfig } from '@ngu/carousel';

export const defaultCarouselTileConfig: NguCarouselConfig = {
  grid: {
    xs: 1,
    sm: 1,
    md: 1,
    lg: 5,
    all: 0
  },
  speed: 250,
  point: {
    visible: true
  },
  touch: true,
  loop: true,
  interval: {
    timing: 1500
  },
  animation: 'lazy'
};
