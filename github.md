---
layout: default
---

<div class="home">

  <h1><i>GitHub</i></h1>

  <ul class="posts">
    {% for post in site.categories.github %}
      <li>
        <span class="post-date">{{ post.date | date: "%b %-d, %Y" }}</span>
        <a class="post-link" href="{{ post.url }}">{{ post.title }}</a>
        <div class="title-desc">{{ post.description }}</div>
      </li>
    {% endfor %}
  </ul>

</div>