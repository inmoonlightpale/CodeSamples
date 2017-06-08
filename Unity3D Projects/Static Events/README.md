Sometimes, you just need static, system-wide events. This implementations is unorthodox, and goes against the grain of subscribing-to-events-from-each-single-origin, but it does have the advantage of being way faster than Unity's events system, AND of being ridiculously flexible.

You can see a practical end-class implementation of the events themselves in the Window Tweens components.