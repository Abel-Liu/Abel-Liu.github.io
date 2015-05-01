module Jekyll

  class TagTplPage < Page
    def initialize(site, base, dir, tag)
      @site = site
      @base = base
      @dir = dir
      @name = 'index.html'

      self.process(@name)
      self.read_yaml(File.join(base, '_layouts'), 'tagtpl.html')
      self.data['tag'] = tag
    end
  end

  class TagTplGenerator < Generator
    safe true

    def generate(site)
      if site.layouts.key? 'tagtpl'
        dir = site.config['tag_dir'] || '_tag'
        site.tags.keys.each do |tag|
          site.pages << TagTplPage.new(site, site.source, File.join(dir, tag), tag)
        end
      end
    end
  end

end