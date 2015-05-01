module Jekyll

  class CategoryTplPage < Page
    def initialize(site, base, dir, category)
      @site = site
      @base = base
      @dir = dir
      @name = 'index.html'

      self.process(@name)
      self.read_yaml(File.join(base, '_layouts'), 'tagtpl.html')
      self.data['tag'] = category
    end
  end

  class CategoryTplGenerator < Generator
    safe true

    def generate(site)
      if site.layouts.key? 'tagtpl'
        dir = site.config['category_dir'] || '_cat'
        site.categories.keys.each do |cat|
          site.pages << CategoryTplPage.new(site, site.source, File.join(dir, cat), cat)
        end
      end
    end
  end

end