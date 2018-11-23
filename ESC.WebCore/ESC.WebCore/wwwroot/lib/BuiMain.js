//布局
var layoutControl = new BUI.Layout.BuiLayout({
    srcNode: "#bui-layout",
    children: [{
        xclass: 'controller',
        srcNode: "#bui-layout-north",
        region: 'north',
        height: 50
    }, {
        srcNode: "#bui-layout-west",
        xclass: 'controller',
        region: 'west',
        width: 230
    }, {
        srcNode: "#bui-layout-center",
        xclass: 'controller',
        region: 'center'
    }]
});
layoutControl.render();

//左侧菜单
var leftMenu = new BUI.Menu.SideMenu({
    render: "#mainMenu"
});
layoutControl.on("resize", function () {
    leftMenu.syncFit();
});
leftMenu.syncFit();