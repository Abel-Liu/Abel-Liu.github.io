$(document).ready(function() {
    $('#fullpage').fullpage({
        sectionsColor: ['#e67e22', '#4BBFC3', '#22C3E6'],
        anchors: ['tag', 'secondPage', 'life'],
        navigation: true,
        navigationPosition: 'right',
        navigationTooltips: ['Tag', 'Second page', 'Life'],
        //scrollOverflow: true,

        afterLoad: function(anchorLink, index) {
            if (index == 3) {
                InLife();
            }
        },
        onLeave: function(index, nextIndex, direction) {
            if (index == 3) {
                OutLife();
            }
        }
    });

    InitTag()
});

function InitTag() {
    var tags = '-- --- ·-· ··· ·,爱徒步,一丝不苟,coding是灵魂,C++很强大C#最优雅,电脑专家,任何事物都有两面性,晨昏定省,爱情坚信者,一切事物都有偶然性和必然性,历史爱好者,IT人,泛游戏爱好者,不dota,考据求证派,讨厌心灵鸡汤,讲规矩,电脑宅,爱洗手,整理控,从不说谎,讨厌不标准读音及错别字及错误标点及错误文章格式,不喜客气,最烦写代码时被打扰,不追星,相比饮料更喜欢白开水,中原人,南方人眼中的北方人,别找我修电脑,微软粉+苹果粉+Google粉,喜欢文言文';
    var arr = tags.split(',');
    var html = '';

    arr.sort(function() {
        return 0.5 - Math.random()
    });

    for (var i = 0; i < arr.length; i++) {
        html += '<span class="about-tag-container" id="about-tag-' + i + '"><span class="about-tag">' + arr[i] + '</span></span>';
    }

    $('#aboutpage-divtop').after(html);

    for (var i = 0; i < arr.length; i++) {
        $('#about-tag-' + i).fadeIn(GetRandomNum(400, 4000));
    }

}

function GetRandomNum(Min, Max) {
    var Range = Max - Min;
    var Rand = Math.random();
    return (Min + Math.round(Rand * Range));
}

function InLife() {
    $('#l1').addClass('animated rotateInDownLeft');
    $('#l2').addClass('animated fadeInDown');
    $('#l3').addClass('animated rotateInDownRight');

    $('#l7').addClass('animated rotateInUpLeft');
    $('#l8').addClass('animated fadeInUp');
    $('#l9').addClass('animated rotateInUpRight');
}

function OutLife() {
    $('#l1').removeClass('animated rotateInDownLeft');
    $('#l2').removeClass('animated fadeInDown');
    $('#l3').removeClass('animated rotateInDownRight');

    $('#l7').removeClass('animated rotateInUpLeft');
    $('#l8').removeClass('animated fadeInUp');
    $('#l9').removeClass('animated rotateInUpRight');
}
