//BUI.Tree.TreeList
(function (BUI, $) {
    "use strict";
    BUI.Tree = {};

    var List = BUI.List,
    Data = BUI.Data,
    EXPAND = "expanded",
    LOADING = "loading",
    CHECKED = "checked",
    PARTIAL_CHECKED = "partial-checked",
    MAP_TYPES = {
        NONE: "none",
        ALL: "all",
        CUSTOM: "custom",
        ONLY_LEAF: "onlyLeaf"
    },
    CLS_ICON = "bui-tree-icon",
    CLS_ELBOW = "bui-tree-elbow",
    CLS_SHOW_LINE = "bui-tree-show-line",
    CLS_ICON_PREFIX = CLS_ELBOW + "-",
    CLS_ICON_WRAPER = CLS_ICON + "-wraper",
    CLS_LINE = CLS_ICON_PREFIX + "line",
    CLS_END = CLS_ICON_PREFIX + "end",
    CLS_EMPTY = CLS_ICON_PREFIX + "empty",
    CLS_EXPANDER = CLS_ICON_PREFIX + "expander",
    CLS_CHECKBOX = CLS_ICON + "-checkbox",
    CLS_RADIO = CLS_ICON + "-radio",
    CLS_EXPANDER_END = CLS_EXPANDER + "-end";

    function makeSureNode(self, node) {
        if (BUI.isString(node)) {
            node = self.findNode(node);
        }
        return node;
    }
   
    var TreeList = List.SimpleList.extend({
        bindUI: function () {
            var _self = this,
            el = _self.get("el"),
            multipleCheck = _self.get("multipleCheck"),
            store = _self.get("store");
            _self.on("itemclick",
            function (ev) {
                var sender = $(ev.domTarget),
                element = ev.element,
                node = ev.item;
                if (sender.hasClass(CLS_EXPANDER)) {
                    _self._toggleExpand(node, element);
                    return false;
                } else if (sender.hasClass(CLS_CHECKBOX)) {
                    var checked = _self.isChecked(node);
                    _self.setNodeChecked(node, !checked);
                    _self.fire("checkedchangeonce", {
                        node: node,
                        checked: !checked
                    });
                } else if (sender.hasClass(CLS_RADIO)) {
                    _self.setNodeChecked(node, true);
                }
            });
            _self.on("itemrendered",
            function (ev) {
                var node = ev.item,
                element = ev.domTarget;
                _self._resetIcons(node, element);
                if (_self.isCheckable(node) && multipleCheck && _self.get("cascadeCheckd")) {
                    _self._resetPatialChecked(node, null, null, element);
                }
                if (_self._isExpanded(node, element)) {
                    _self._showChildren(node);
                }
            });
            _self._initExpandEvent();

            if (!store) {
                return;
            }
            store.on("load",
            function (e) {
                _self.onLoad(e);
            });
            store.on("add",
            function (e) {
                _self.onAdd(e);
            });
            store.on("remove",
            function (e) {
                _self.onRemove(e);
            });
            store.on("update",
            function (e) {
                _self.onUpdate(e);
            });
        },
        onLoad: function (e) {
            var _self = this,
            store = _self.get("store"),
            root = store.get("root"),
            node;
            if (!e || e.node == root) {
                _self._initRoot();
            }
            if (e && e.node) {
                _self._loadNode(e.node);
            }
        },
        _initRoot: function () {
            var _self = this,
            store = _self.get("store"),
            root,
            showRoot = _self.get("showRoot"),
            nodes;
            if (store) {
                root = store.get("root");
                _self.setInternal("root", root);
                if (showRoot) {
                    nodes = [root];
                } else {
                    nodes = root.children;
                }
                BUI.each(nodes,
                function (subNode) {
                    _self._initChecked(subNode, true);
                });
                _self.clearItems();
                _self.addItems(nodes);
            }
        },
        _initChecked: function (node, deep) {
            var _self = this,
            checkType = _self.get("checkType"),
            checkedField = _self.get("checkedField"),
            multipleCheck = _self.get("multipleCheck"),
            checkableField = _self.get("checkableField"),
            cascadeCheckd = _self.get("cascadeCheckd"),
            parent;
            if (checkType === MAP_TYPES.NONE) {
                node[checkableField] = false;
                node[checkedField] = false;
                return;
            }
            if (checkType === MAP_TYPES.ONLY_LEAF) {
                if (node.leaf) {
                    node[checkableField] = true;
                } else {
                    node[checkableField] = false;
                    node[checkedField] = false;
                    if (deep) {
                        BUI.each(node.children,
                        function (subNode) {
                            _self._initChecked(subNode, deep);
                        });
                    }
                }
                return;
            }
            if (checkType === MAP_TYPES.CUSTOM) {
                if (node[checkableField] == null) {
                    node[checkableField] = node[checkedField] != null;
                }
            }
            if (checkType === MAP_TYPES.ALL) {
                node[checkableField] = true;
            }
            if (!node || !_self.isCheckable(node)) {
                return;
            }
            parent = node.parent;
            if (!_self.isChecked(node) && cascadeCheckd) {
                if (parent && _self.isChecked(parent)) {
                    if (multipleCheck || !_self._hasChildChecked(parent)) {
                        _self.setStatusValue(node, "checked", true);
                    }
                }
                if (node.children && node.children.length && _self._isAllChildrenChecked(node) || !multipleCheck && _self._hasChildChecked(node)) {
                    _self.setStatusValue(node, "checked", true);
                }
            }
            if (deep) {
                BUI.each(node.children,
                function (subNode) {
                    _self._initChecked(subNode, deep);
                });
            }
        },
        isCheckable: function (node) {
            return node[this.get("checkableField")];
        },
        isChecked: function (node) {
            if (!node) {
                return false;
            }
            return !!node[this.get("checkedField")];
        },
        _hasChildChecked: function (node) {
            if (!node || node.leaf) {
                return false;
            }
            var _self = this;
            return _self.getCheckedNodes(node).length != 0;
        },
        getCheckedNodes: function (parent) {
            var _self = this,
            store = _self.get("store");
            return store.findNodesBy(function (node) {
                return _self.isChecked(node);
            },
            parent);
        },
        _isAllChildrenChecked: function (node) {
            if (!node || node.leaf) {
                return false;
            }
            var _self = this,
            children = node.children,
            rst = true;
            BUI.each(children,
            function (subNode) {
                rst = rst && _self.isChecked(subNode);
                if (!rst) {
                    return false;
                }
            });
            return rst;
        },
        _loadNode: function (node) {
            var _self = this;
            _self._initChecked(node, true);
            _self.expandNode(node);
            _self._updateIcons(node);
            _self.setItemStatus(node, LOADING, false);
        },
        expandNode: function (node, deep) {
            var _self = this,
            element;
            if (BUI.isString(node)) {
                node = _self.findNode(node);
            }
            if (!node) {
                return;
            }
            if (node.parent && !_self.isExpanded(node.parent)) {
                _self.expandNode(node.parent);
            }
            element = _self.findElement(node);
            _self._expandNode(node, element, deep);
        },
        findNode: function (id, parent) {
            return this.get("store").findNode(id, parent);
        },
        _expandNode: function (node, element, deep) {
            var _self = this,
            accordion = _self.get("accordion"),
            store = _self.get("store");
            if (node.leaf) {
                return;
            }
            if (!_self.hasStatus(node, EXPAND, element)) {
                if (accordion && node.parent) {
                    var slibings = node.parent.children;
                    BUI.each(slibings,
                    function (sNode) {
                        if (sNode != node) {
                            _self.collapseNode(sNode);
                        }
                    });
                }
                if (store && !store.isLoaded(node)) {
                    if (!_self._isLoading(node, element)) {
                        store.loadNode(node);
                    }
                } else if (element) {
                    _self.setItemStatus(node, EXPAND, true, element);
                    _self._showChildren(node);
                    _self.fire("expanded", {
                        node: node,
                        element: element
                    });
                }
            }
            BUI.each(node.children,
            function (subNode) {
                if (deep || _self.isExpanded(subNode)) {
                    _self.expandNode(subNode, deep);
                }
            });
        },
        collapseNode: function (node) {
            var _self = this,
            element;
            if (BUI.isString(node)) {
                node = _self.findNode(node);
            }
            if (!node) {
                return;
            }
            element = _self.findElement(node);
            _self._collapseNode(node, element);
        },
        _collapseNode: function (node, element, deep) {
            var _self = this;
            if (node.leaf) {
                return;
            }
            if (_self.hasStatus(node, EXPAND, element)) {
                _self.setItemStatus(node, EXPAND, false, element);
                if (deep) {
                    _self._collapseChildren(node, deep);
                    _self.removeItems(node.children);
                } else {
                    _self._hideChildrenNodes(node);
                }
                _self.fire("collapsed", {
                    node: node,
                    element: element
                });
            }
        },
        _collapseChildren: function (parentNode, deep) {
            var _self = this,
            children = parentNode.children;
            BUI.each(children,
            function (node) {
                _self.collapseNode(node, deep);
            });
        },
        _hideChildrenNodes: function (node) {
            var _self = this,
            children = node.children,
            elements = [];
            BUI.each(children,
            function (subNode) {
                var element = _self.findElement(subNode);
                if (element) {
                    elements.push(element);
                    _self._hideChildrenNodes(subNode);
                }
            });
            if (_self.get("expandAnimate")) {
                elements = $(elements);
                elements.animate({
                    height: 0
                },
                function () {
                    _self.removeItems(children);
                });
            } else {
                _self.removeItems(children);
            }
        },
        _updateIcons: function (node) {
            var _self = this,
            element = _self.findElement(node);
            if (element) {
                _self._resetIcons(node, element);
                if (_self._isExpanded(node, element) && !node.leaf) {
                    BUI.each(node.children,
                    function (subNode) {
                        _self._updateIcons(subNode);
                    });
                }
            }
        },
        _resetIcons: function (node, element) {
            if (!this.get("showIcons")) {
                return;
            }
            var _self = this,
            iconContainer = _self.get("iconContainer"),
            containerEl,
            iconsTpl = _self._getIconsTpl(node);
            $(element).find("." + CLS_ICON_WRAPER).remove();
            containerEl = $(element).find(iconContainer).first();
            if (iconContainer && containerEl.length) {
                $(iconsTpl).prependTo(containerEl);
            } else {
                $(element).prepend($(iconsTpl));
            }
        },
        _getIconsTpl: function (node) {
            var _self = this,
            level = node.level,
            start = _self.get("startLevel"),
            iconWraperTpl = _self.get("iconWraperTpl"),
            icons = [],
            i;
            for (i = start; i < level; i = i + 1) {
                icons.push(_self._getLevelIcon(node, i));
            }
            icons.push(_self._getExpandIcon(node));
            icons.push(_self._getCheckedIcon(node));
            icons.push(_self._getNodeTypeIcon(node));
            return BUI.substitute(iconWraperTpl, {
                icons: icons.join("")
            });
        },
        _getLevelIcon: function (node, level) {
            var _self = this,
            showLine = _self.get("showLine"),
            cls = CLS_EMPTY,
            levelNode;
            if (showLine) {
                if (node.level === level || level == null) {
                    cls = _self._isLastNode(node) ? CLS_END : CLS_ELBOW;
                } else {
                    levelNode = _self._getParentNode(node, level);
                    cls = _self._isLastNode(levelNode) ? CLS_EMPTY : CLS_LINE;
                }
            }
            return _self._getIcon(cls);
        },
        _getExpandIcon: function (node) {
            var _self = this,
            cls = CLS_EXPANDER;
            if (node.leaf) {
                return _self._getLevelIcon(node);
            }
            if (_self._isLastNode(node)) {
                cls = cls + " " + CLS_EXPANDER_END;
            }
            return _self._getIcon(cls);
        },
        _getCheckedIcon: function (node) {
            var _self = this,
            checkable = _self.isCheckable(node),
            cls;
            if (checkable) {
                cls = _self.get("multipleCheck") ? CLS_CHECKBOX : CLS_RADIO;
                return _self._getIcon(cls);
            }
            return "";
        },
        _getNodeTypeIcon: function (node) {
            var _self = this,
            cls = node.cls ? node.cls : node.leaf ? _self.get("leafCls") : _self.get("dirCls");
            return _self._getIcon(cls);
        },
        _getIcon: function (cls) {
            var _self = this,
            iconTpl = _self.get("iconTpl");
            return BUI.substitute(iconTpl, {
                cls: cls
            });
        },
        _getParentNode: function (node, level) {
            var nodeLevel = node.level,
            parent = node.parent,
            i = nodeLevel - 1;
            if (nodeLevel <= level) {
                return null;
            }
            while (i > level) {
                parent = parent.parent;
                i = i - 1;
            }
            return parent;
        },
        _isLastNode: function (node) {
            if (!node) {
                return false;
            }
            if (node == this.get("root")) {
                return true;
            }
            var _self = this,
            parent = node.parent,
            siblings = parent ? parent.children : _self.get("nodes"),
            count;
            count = siblings.length;
            return siblings[count - 1] === node;
        },
        _isExpanded: function (node, element) {
            return this.hasStatus(node, EXPAND, element);
        },
        collapseAll: function () {
            var _self = this,
            elements = _self.getAllElements();
            BUI.each(elements,
            function (element) {
                var item = _self.getItemByElement(element);
                if (item) {
                    _self._collapseNode(item, element, true);
                }
            });
        },
        expandAll: function () {
            var _self = this,
            elements = _self.getAllElements();
            BUI.each(elements,
            function (element) {
                var item = _self.getItemByElement(element);
                _self._expandNode(item, element, true);
            });
        },
        expandPath: function (path, async, startIndex) {
            if (!path) {
                return;
            }
            startIndex = startIndex || 0;
            var _self = this,
            store = _self.get("store"),
            preNode,
            node,
            i,
            id,
            arr = path.split(",");
            preNode = _self.findNode(arr[startIndex]);
            for (i = startIndex + 1; i < arr.length; i++) {
                id = arr[i];
                node = _self.findNode(id, preNode);
                if (preNode && node) {
                    _self.expandNode(preNode);
                    preNode = node;
                } else if (preNode && async) {
                    store.load({
                        id: preNode.id
                    },
                    function () {
                        node = _self.findNode(id, preNode);
                        if (node) {
                            _self.expandPath(path, async, i);
                        }
                    });
                    break;
                }
            }
        },
        getCheckedLeaf: function (parent) {
            var _self = this,
            store = _self.get("store");
            return store.findNodesBy(function (node) {
                return node.leaf && _self.isChecked(node);
            },
            parent);
        },
        isItemSelectable: function (item) {
            var _self = this,
            dirSelectable = _self.get("dirSelectable"),
            node = item;
            if (node && !dirSelectable && !node.leaf) {
                return false;
            }
            return true;
        },
        isExpanded: function (node) {
            if (!node || node.leaf) {
                return false;
            }
            var _self = this,
            element;
            if (_self._isRoot(node) && !_self.get("showRoot")) {
                return true;
            }
            if (BUI.isString(node)) {
                item = _self.getItem(node);
            }
            element = _self.findElement(node);
            return this._isExpanded(node, element);
        },
        toggleExpand: function (node) {
            var _self = this,
            element;
            if (BUI.isString(node)) {
                item = _self.getItem(node);
            }
            element = _self.findElement(node);
            _self._toggleExpand(node, element);
        },
        setNodeChecked: function (node, checked, deep) {
            deep = deep == null ? true : deep;
            if (!node) {
                return;
            }
            var _self = this,
            parent, multipleCheck = _self.get("multipleCheck"),
            cascadeCheckd = _self.get("cascadeCheckd"),
            element;
            node = makeSureNode(this, node);
            if (!node) {
                return;
            }
            parent = node.parent;
            if (!_self.isCheckable(node)) {
                return;
            }
            if (_self.isChecked(node) !== checked || _self.hasStatus(node, "checked") !== checked) {
                element = _self.findElement(node);
                if (cascadeCheckd) {
                    if (element) {
                        _self.setItemStatus(node, CHECKED, checked, element);
                        if (multipleCheck) {
                            _self._resetPatialChecked(node, checked, checked, element);
                        } else {
                            if (checked && parent && _self.isChecked(parent) != checked) {
                                _self.setNodeChecked(parent, checked, false);
                            }
                        }
                    } else if (!_self.isItemDisabled(node)) {
                        _self.setStatusValue(node, CHECKED, checked);
                    }
                    if (parent) {
                        if (_self.isChecked(parent) != checked) {
                            _self._resetParentChecked(parent);
                        } else if (multipleCheck) {
                            _self._resetPatialChecked(parent, null, null, null, true);
                        }
                    }
                } else if (!_self.isItemDisabled(node)) {
                    if (element) {
                        _self.setItemStatus(node, CHECKED, checked, element);
                    } else {
                        _self.setStatusValue(node, CHECKED, checked);
                    }
                }
                if (checked && !multipleCheck && (_self.isChecked(parent) || parent == _self.get("root") || !cascadeCheckd)) {
                    var nodes = parent.children;
                    BUI.each(nodes,
                    function (slibNode) {
                        if (slibNode !== node && _self.isChecked(slibNode)) {
                            _self.setNodeChecked(slibNode, false);
                        }
                    });
                }
                _self.fire("checkedchange", {
                    node: node,
                    element: element,
                    checked: checked
                });
            }
            if (!node.leaf && deep && cascadeCheckd) {
                BUI.each(node.children,
                function (subNode, index) {
                    if (multipleCheck || !checked || !multipleCheck && index == 0) {
                        _self.setNodeChecked(subNode, checked, deep);
                    }
                });
            }
        },
        setChecked: function (node) {
            this.setNodeChecked(node, true);
        },
        clearAllChecked: function () {
            var _self = this,
            nodes = _self.getCheckedNodes();
            BUI.each(nodes,
            function (node) {
                _self.setNodeChecked(node, false);
            });
        },
        _resetPatialChecked: function (node, checked, hasChecked, element, upper) {
            if (!node || node.leaf) {
                return true;
            }
            var _self = this,
            hasChecked;
            checked = checked == null ? _self.isChecked(node) : checked;
            if (checked) {
                _self.setItemStatus(node, PARTIAL_CHECKED, false, element);
                return;
            }
            hasChecked = hasChecked == null ? _self._hasChildChecked(node) : hasChecked;
            _self.setItemStatus(node, PARTIAL_CHECKED, hasChecked, element);
            if (upper && node.parent) {
                _self._resetPatialChecked(node.parent, false, hasChecked ? hasChecked : null, null, upper);
            }
        },
        _resetParentChecked: function (parentNode) {
            if (!this.isCheckable(parentNode)) {
                return;
            }
            var _self = this,
            multipleCheck = _self.get("multipleCheck"),
            allChecked = multipleCheck ? _self._isAllChildrenChecked(parentNode) : _self._hasChildChecked(parentNode);
            _self.setStatusValue(parentNode, "checked", allChecked);
            _self.setNodeChecked(parentNode, allChecked, false);
            multipleCheck && _self._resetPatialChecked(parentNode, allChecked, null, null, true);
        },
        _initExpandEvent: function () {
            var _self = this,
            el = _self.get("el"),
            expandEvent = _self.get("expandEvent"),
            collapseEvent = _self.get("collapseEvent");
            function createCallback(methodName) {
                return function (ev) {
                    var sender = $(ev.domTarget),
                    element = ev.element,
                    node = ev.item;
                    if (!sender.hasClass(CLS_EXPANDER)) {
                        _self[methodName](node, element);
                    }
                };
            }
            if (expandEvent == collapseEvent) {
                _self.on(expandEvent, createCallback("_toggleExpand"));
            } else {
                if (expandEvent) {
                    _self.on(expandEvent, createCallback("_expandNode"));
                }
                if (collapseEvent) {
                    _self.on(collapseEvent, createCallback("_collapseNode"));
                }
            }
        },
        _isForceChecked: function (node) {
            var _self = this,
            multipleCheck = _self.get("multipleCheck");
            return multipleCheck ? _self._isAllChildrenChecked() : _isForceChecked();
        },
        _isRoot: function (node) {
            var _self = this,
            store = _self.get("store");
            if (store && store.get("root") == node) {
                return true;
            }
            return false;
        },
        _setLoadStatus: function (node, element, loading) {
            var _self = this;
            _self.setItemStatus(node, LOADING, loading, element);
        },
        _addNode: function (node, index) {
            var _self = this,
            parent = node.parent,
            scount, prevNode, nextNode, cIndex;
            _self._initChecked(node, true);
            if (parent) {
                if (_self.isExpanded(parent)) {
                    scount = parent.children.length;
                    cIndex = _self._getInsetIndex(node);
                    _self.addItemAt(node, cIndex);
                    if (index == scount - 1 && index > 0) {
                        prevNode = parent.children[index - 1];
                        _self._updateIcons(prevNode);
                    }
                }
                _self._updateIcons(parent);
            } else {
                cIndex = _self._getInsetIndex(node);
                _self.addItemAt(node, cIndex);
                prevNode = _self.get("nodes")[index - 1];
                _self._updateIcons(prevNode);
            }
        },
        _getInsetIndex: function (node) {
            var _self = this,
            nextNode, rst = null;
            nextNode = _self._getNextItem(node);
            if (nextNode) {
                return _self.indexOfItem(nextNode);
            }
            return _self.getItemCount();
        },
        _getNextItem: function (item) {
            var _self = this,
            parent = item.parent,
            slibings, cIndex, rst = null;
            if (!parent) {
                return null;
            }
            slibings = parent.children;
            cIndex = BUI.Array.indexOf(item, slibings);
            rst = slibings[cIndex + 1];
            return rst || _self._getNextItem(parent);
        },
        onAdd: function (e) {
            var _self = this,
            node = e.node,
            index = e.index;
            _self._addNode(node, index);
        },
        _updateNode: function (node) {
            var _self = this;
            _self.updateItem(node);
            _self._updateIcons(node);
        },
        onUpdate: function (e) {
            var _self = this,
            node = e.node;
            _self._updateNode(node);
        },
        _removeNode: function (node, index) {
            var _self = this,
            parent = node.parent,
            scount, prevNode;
            _self.collapseNode(node);
            if (!parent) {
                return;
            }
            _self.removeItem(node);
            if (_self.isExpanded(parent)) {
                scount = parent.children.length;
                if (scount == index && index !== 0) {
                    prevNode = parent.children[index - 1];
                    _self._updateIcons(prevNode);
                }
            }
            _self._updateIcons(parent);
            _self._resetParentChecked(parent);
        },
        onRemove: function (e) {
            var _self = this,
            node = e.node,
            index = e.index;
            _self._removeNode(node, index);
        },
        _showChildren: function (node) {
            if (!node || !node.children) {
                return;
            }
            var _self = this,
            index = _self.indexOfItem(node),
            length = node.children.length,
            subNode,
            i = length - 1,
            elements = [];
            for (i = length - 1; i >= 0; i--) {
                subNode = node.children[i];
                if (!_self.getItem(subNode)) {
                    if (_self.get("expandAnimate")) {
                        el = _self._addNodeAt(subNode, index + 1);
                        el.hide();
                        el.slideDown();
                    } else {
                        _self.addItemAt(subNode, index + 1);
                    }
                }
            }
        },
        _addNodeAt: function (item, index) {
            var _self = this,
            items = _self.get("items");
            if (index === undefined) {
                index = items.length;
            }
            items.splice(index, 0, item);
            return _self.addItemToView(item, index);
        },
        _isLoading: function (node, element) {
            var _self = this;
            return _self.hasStatus(node, LOADING, element);
        },
        _toggleExpand: function (node, element) {
            var _self = this;
            if (_self._isExpanded(node, element)) {
                _self._collapseNode(node, element);
            } else {
                _self._expandNode(node, element);
            }
        },
        _uiSetShowRoot: function (v) {
            var _self = this,
            start = this.get("showRoot") ? 0 : 1;
            _self.set("startLevel", start);
        },
        _uiSetShowLine: function (v) {
            var _self = this,
            el = _self.get("el");
            if (v) {
                el.addClass(CLS_SHOW_LINE);
            } else {
                el.removeClass(CLS_SHOW_LINE);
            }
        },
        getSelection: function () {
            var _self = this,
            field = _self.getStatusField("selected"),
            store;
            if (field) {
                store = _self.get("store");
                return store.findNodesBy(function (node) {
                    return node[field];
                });
            }
            return List.SimpleList.prototype.getSelection.call(this);
        },
        getSelected: function () {
            var _self = this,
            field = _self.getStatusField("selected"),
            store;
            if (field) {
                store = _self.get("store");
                return store.findNodeBy(function (node) {
                    return node[field];
                });
            }
            return List.SimpleList.prototype.getSelected.call(this);
        }
    },
    {
        ATTRS: {
            store: {
                value: {}
            },
            root: {},
            iconContainer: {},
            iconWraperTpl: {
                value: '<span class="' + CLS_ICON_WRAPER + '">{icons}</span>'
            },
            showLine: {
                value: false
            },
            showIcons: {
                value: true
            },
            iconTpl: {
                value: '<span class="bui-tree-icon {cls}"></span>'
            },
            leafCls: {
                value: CLS_ICON_PREFIX + "leaf"
            },
            dirCls: {
                value: CLS_ICON_PREFIX + "dir"
            },
            checkType: {
                share:false,
                value: "custom"
            },
            cascadeCheckd: {
                value: true
            },
            accordion: {
                value: false
            },
            multipleCheck: {
                value: true
            },
            checkedField: {
                valueFn: function () {
                    return this.getStatusField("checked");
                }
            },
            checkableField: {
                value: "checkable"
            },
            itemStatusFields: {
                value: {
                    expanded: "expanded",
                    disabled: "disabled",
                    checked: "checked"
                }
            },
            dirSelectable: {
                value: true
            },
            showRoot: {
                value: false
            },
            events: {
                value: {
                    expanded: false,
                    collapsed: false,
                    checkedchange: false,
                    checkedchangeonce:false
                }
            },
            expandEvent: {
                value: "itemdblclick"
            },
            expandAnimate: {
                value: false
            },
            collapseEvent: {
                value: "itemdblclick"
            },
            startLevel: {
                value: 1
            },
            itemCls: {
                value: BUI.prefix + "tree-item"
            },
            itemTpl: {
                value: "<li>{text}</li>"
            },
            idField: {
                value: "id"
            },
            items: {
                sync: false
            }
        }
    },
    {
        xclass: "tree-list"
    });
    BUI.Tree.TreeList = TreeList;
})(window.BUI, jQuery);