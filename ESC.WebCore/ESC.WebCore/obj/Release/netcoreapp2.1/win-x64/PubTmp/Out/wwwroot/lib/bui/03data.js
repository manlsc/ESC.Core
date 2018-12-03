//BUI.Data.Store +
(function (BUI) {
    "use strict";
    BUI.Data = {};
    var ASC = "ASC",
		DESC = "DESC";
    /**
	 * 删除数组指定索引元素
	 * @param {Object} index
	 * @param {Object} array
	 */
    function removeAt(index, array) {
        if (index < 0) {
            return;
        }
        var records = array,
			record = records[index];
        records.splice(index, 1);
        return record;
    }

    /**
	 * 删除数组指定元素
	 * @param {Object} record
	 * @param {Object} array
	 */
    function removeFrom(record, array) {
        var index = BUI.Array.indexOf(record, array);
        if (index >= 0) {
            removeAt(index, array);
        }
    }

    /**
	 * 是否包含指定元素
	 * @param {Object} record
	 * @param {Object} array
	 */
    function contains(record, array) {
        return BUI.Array.indexOf(record, array) !== -1;
    }


    var store = function (config) {
        store.superclass.constructor.call(this, config);
        this._init();
    };
    store.ATTRS = {
        /**
		 * 比较函数
		 * @cfg {Function} compareFunction
		 * 函数原型 function(v1,v2)，比较2个字段是否相等
		 * 如果是字符串则按照本地比较算法，否则使用 > ,== 验证
		 */
        compareFunction: {
            value: function (v1, v2) {
                if (v1 === undefined) {
                    v1 = "";
                }
                if (v2 === undefined) {
                    v2 = "";
                }
                if (BUI.isString(v1)) {
                    return v1.localeCompare(v2);
                }
                if (v1 > v2) {
                    return 1;
                } else if (v1 === v2) {
                    return 0;
                } else {
                    return -1;
                }
            }
        },
        /**
		 * 排序字段
		 * @type {String}
		 */
        sortField: {},
        /**
		 * 排序方向,'ASC'、'DESC'
		 * @type {String}
		 */
        sortDirection: {
            value: "ASC"
        },
        /**
		 * 对比2个对象是否相当，在去重、更新、删除，查找数据时使用此函数
		 * @type {Object}
		 * 
		 */
        matchFunction: {
            value: function (obj1, obj2) {
                return obj1 == obj2;
            }
        },       
        /**
		 * 事件
		 */
        events: {
            value: ["load", "add", "remove", "update"]
        },
        /**
		 * 上次查询的参数
		 * @type {Object}
		 * @readOnly
		 */
        lastParams: {
            shared: false,
            value: {}
        },
        /**
		 * 参数
		 */
        params: {},
        /**
		 * 本地数据源
		 * @type {Array}
		 */
        data: {
            value: []
        },
        /**
		 * 缓存的数据，包含以下几个字段
		 * <ol>
		 * <li>rows: 数据集合</li>
		 * <li>results: 总的数据条数</li>
		 * </ol>
		 * @type {Object}
		 * @private
		 * @readOnly
		 */
        resultMap: {
            shared: false,
            value: {
                total: 0,
                rows: []
            }
        },
        /**
		 * 加载数据时，返回数据的根目录
		 */
        root: {
            value: "rows"
        },
        /**
		 * 加载数据时，返回记录的总数的字段，用于分页
		 */
        totalProperty: {
            value: "total"
        },
        /**
		 * 删除掉的纪录
		 * @readOnly
		 * @private
		 * @type {Array}
		 */
        deletedRecords: {
            shared: false,
            value: []
        },
        /**
		 * 更改的纪录集合
		 * @type {Array}
		 * @private
		 * @readOnly
		 */
        modifiedRecords: {
            shared: false,
            value: []
        },
        /**
		 * 新添加的纪录集合，只读
		 * @type {Array}
		 * @private
		 * @readOnly
		 */
        newRecords: {
            shared: false,
            value: []
        },
        /**
		 * 页码
		 */
        pageIndex: {
            value: 1
        },
        /**
		 *  每页多少条记录,默认为null,此时不分页，当指定了此值时分页
		 */
        pageSize: {
            value: 20
        },
        /**
		 * 首付自动查询
		 */
        autoLoad: {
            value: true
        },
        /**
		 * 是否ajax
		 */
        isAjax: {
            value: true
        },
        /**
		 * 匹配的字段名
		 * @type {Array}
		 */
        matchFields: {
            value: []
        }
    };
    BUI.extend(store, BUI.Base);
    BUI.augment(store, {
        isStore: true,
        /**
		 * 初始化
		 */
        _init: function () {
            var _self = this,
				autoLoad = _self.get("autoLoad");
            _self._initParams();
            if (autoLoad) {
                _self.load();
            } else {
                _self._setResult([]);
            }
        },
        /**
		 * 初始化参数
		 */
        _initParams: function () {
            var _self = this,
				lastParams = _self.get("lastParams"),
				params = _self.get("params");
            BUI.mix(lastParams, params);
        },
        /**
		 * 根据特定参数进行查询
		 * @param {Object} params
		 * @param {Object} callback
		 */
        load: function (params, callback) {
            var _self = this,
				sortField = _self.get("sortField"),
				sortDirection = _self.get("sortDirection"),
				lastParams = _self.get("lastParams");

            BUI.mix(lastParams, _self.getAppendParams(), params);
            params = BUI.cloneObject(lastParams);
            //如果ajax请求,则自定义查询,否则本地查询
            if (_self.get("isAjax")) {
                _self.onAjax(params, callback);
            } else {
                //根据参数查询结果集
                var data = _self._getMatches(params),
                    rows = [];
                //根据排序字段对结果集进行排序
                _self.sortData(sortField, sortDirection, data);
                //触发onload事件
                _self.onLoad(data, params);

                if (callback) {
                    if (params.pageSize) {
                        var end = parseInt(params.pageIndex) * parseInt(params.pageSize);
                        rows = data.slice(end - param.pageSize, end);
                        callback({
                            rows: rows,
                            total: data.length
                        });
                    } else {
                        callback(data, params);
                    }
                }
            }
        },
        onAjax: function (params, callback) { },
        /**
         * 触发onload事件
         * @param data 结果集
         * @param params 参数
         */ 
        onLoad: function (data, params) {
            var _self = this,
				root = _self.get("root"),
				pageSize = params.pageSize;
            if (pageSize) {
                _self.set("pageIndex", params.pageIndex);
            }
            _self._setResult(data);

            _self.fire("load", {
                params: params
            });
        },
        /**
         * 获取扩展参数
         */ 
        getAppendParams: function () {
            var _self = this,
				pageSize = _self.get("pageSize"),
				pageIndex = _self.get("pageIndex") || 1;
            var params = {
                pageSize: pageSize,
                pageIndex: pageIndex
            };
            return params;
        },
        /**
		 * 添加记录
		 * @param {Object} data
		 * @param {Object} noRepeat
		 * @param {Object} match
		 */
        add: function (data, noRepeat, match) {
            var _self = this,
				count = _self.getCount();
            _self.addAt(data, count, noRepeat, match);
        },
        /**
		 * 添加记录到指定索引
		 * @param {Object} data
		 * @param {Object} index
		 * @param {Object} noRepeat
		 * @param {Object} match
		 */
        addAt: function (data, index, noRepeat, match) {
            var _self = this;
            match = match || _self._getDefaultMatch();
            if (!BUI.isArray(data)) {
                data = [data];
            }
            $.each(data,
				function (pos, element) {
				    if (!noRepeat || !_self.contains(element, match)) {
				        _self._addRecord(element, pos + index);
				        _self.get("newRecords").push(element);
				        removeFrom(element, _self.get("deletedRecords"));
				        removeFrom(element, _self.get("modifiedRecords"));
				    }
				});
        },
        _getDefaultMatch: function () {
            return this.get("matchFunction");
        },
        /**
		 * 判断是否包含指定记录
		 * @param {Object} record
		 * @param {Object} match
		 */
        contains: function (record, match) {
            return this.findIndexBy(record, match) !== -1;
        },
        /**
		 * 查找数据所在的索引位置,若不存在返回-1
		 * <pre><code>
		 *  var index = store.findIndexBy(obj);
		 *
		 *  var index = store.findIndexBy(obj,function(obj1,obj2){
		 *    return obj1.id == obj2.id;
		 *  });
		 * </code></pre>
		 * @param {Object} target 指定的记录
		 * @param {Function} [match = matchFunction] @see {BUI.Data.Store#matchFunction}默认为比较2个对象是否相同
		 * @return {Number}
		 */
        findIndexBy: function (target, match) {
            var _self = this,
				position = -1,
				records = _self.getResult();
            match = match || _self._getDefaultMatch();
            if (target === null || target === undefined) {
                return -1;
            }
            $.each(records,
				function (index, record) {
				    if (match(target, record)) {
				        position = index;
				        return false;
				    }
				});
            return position;
        },
        _addRecord: function (record, index) {
            var records = this.getResult();
            if (index == undefined) {
                index = records.length;
            }
            records.splice(index, 0, record);
            this.fire("add", {
                record: record,
                index: index
            });
        },
        /**
		 * 删除记录
		 * @param {Object} data
		 * @param {Object} match
		 */
        remove: function (data, match) {
            var _self = this,
				delData = [];
            match = match || _self._getDefaultMatch();
            if (!BUI.isArray(data)) {
                data = [data];
            }
            $.each(data,
				function (index, element) {
				    var index = _self.findIndexBy(element, match),
						record = removeAt(index, _self.getResult());
				    if (!contains(record, _self.get("newRecords")) && !contains(record, _self.get("deletedRecords"))) {
				        _self.get("deletedRecords").push(record);
				    }
				    removeFrom(record, _self.get("newRecords"));
				    removeFrom(record, _self.get("modifiedRecords"));
				    _self.fire("remove", {
				        record: record
				    });
				});
        },
        /**
		 * 更新记录
		 * @param {Object} obj
		 * @param {Object} isMatch
		 * @param {Object} match
		 */
        update: function (obj, isMatch, match) {
            var record = obj,
				_self = this,
				match = null,
				index = null;
            if (isMatch) {
                match = match || _self._getDefaultMatch();
                index = _self.findIndexBy(obj, match);
                if (index >= 0) {
                    record = _self.getResult()[index];
                }
            }
            record = BUI.mix(record, obj);
            if (!contains(record, _self.get("newRecords")) && !contains(record, _self.get("modifiedRecords"))) {
                _self.get("modifiedRecords").push(record);
            }
            _self.fire("update", {
                record: record
            });
        },
        /**
		 * 获取当前缓存的纪录
		 */
        getResult: function () {
            var _self = this,
				resultMap = _self.get("resultMap"),
				root = _self.get("root");
            return BUI.getValue(resultMap, root);
        },
        /**
		 * 查找记录，仅返回第一条
		 * <pre><code>
		 *  var record = store.find('id','123');
		 * </code></pre>
		 * @param {String} field 字段名
		 * @param {String} value 字段值
		 * @return {Object|null}
		 */
        find: function (field, value) {
            var _self = this,
				result = null,
				records = _self.getResult();
            $.each(records,
				function (index, record) {
				    if (record[field] === value) {
				        result = record;
				        return false;
				    }
				});
            return result;
        },
        /**
		 * 查找记录，返回所有符合查询条件的记录
		 * <pre><code>
		 *   var records = store.findAll('type','0');
		 * </code></pre>
		 * @param {String} field 字段名
		 * @param {String} value 字段值
		 * @return {Array}
		 */
        findAll: function (field, value) {
            var _self = this,
				result = [],
				records = _self.getResult();
            $.each(records,
				function (index, record) {
				    if (record[field] === value) {
				        result.push(record);
				    }
				});
            return result;
        },
        /**
		 * 根据索引查找记录
		 * <pre><code>
		 *  var record = store.findByIndex(1);
		 * </code></pre>
		 * @param {Number} index 索引
		 * @return {Object} 查找的记录
		 */
        findByIndex: function (index) {
            return this.getResult()[index];
        },
        /**
		 * 获取下一条记录
		 * <pre><code>
		 *  var record = store.findNextRecord(obj);
		 * </code></pre>
		 * @param {Object} record 当前记录
		 * @return {Object} 下一条记录
		 */
        findNextRecord: function (record) {
            var _self = this,
				index = _self.findIndexBy(record);
            if (index >= 0) {
                return _self.findByIndex(index + 1);
            }
            return;
        },
        /**
		 * 获取缓存的记录数
		 * <pre><code>
		 *  var count = store.getCount(); //缓存的数据数量
		 *
		 *  var totalCount = store.getTotalCount(); //数据的总数，如果有分页时，totalCount != count
		 * </code></pre>
		 * @return {Number} 记录数
		 */
        getCount: function () {
            return this.getResult().length;
        },
        /**
		 * 获取数据源的数据总数，分页时，当前仅缓存当前页数据
		 * <pre><code>
		 *  var count = store.getCount(); //缓存的数据数量
		 *
		 *  var totalCount = store.getTotalCount(); //数据的总数，如果有分页时，totalCount != count
		 * </code></pre>
		 * @return {Number} 记录的总数
		 */
        getTotalCount: function () {
            var _self = this,
				resultMap = _self.get("resultMap"),
				total = _self.get("totalProperty"),
				totalVal = BUI.getValue(resultMap, total);
            return parseInt(totalVal, 10) || 0;
        },
        /**
		 * 是否包含数据
		 * @return {Boolean} 
		 */
        hasData: function () {
            return this.getCount() !== 0;
        },
        /**
		 * 获取未保存的数据
		 */
        getDirtyData: function () {
            var _self = this;
            return {
                add: _self.get("newRecords"),
                update: _self.get("modifiedRecords"),
                remove: _self.get("deletedRecords")
            };
        },
        /**
		 * 按照指定字段和方向排序
		 * @param {Object} field
		 * @param {Object} direction
		 */
        sort: function (field, direction) {
            var _self = this;
            _self._sortData(field, direction);
        },
        setResult: function (data) {
            var _self = this,
				root = _self.get("root"),
				totalProperty = _self.get("totalProperty"),
				lastParams = _self.get("lastParams");
            if (BUI.isArray(data)) {
                _self.set("data", data);
                _self._setResult(data);
            } else {
                _self.set("data", BUI.getValue(data, root));
                _self._setResult(BUI.getValue(data, root), BUI.getValue(data, totalProperty));
            }

            var params = BUI.cloneObject(lastParams),
				pageSize = params.pageSize;

            if (pageSize) {
                _self.set("pageIndex", params.pageIndex);
            }

            _self.fire("load", {
                params: params
            });
        },
        /**
		 * 计算指定字段的和
		 * @param {Object} field
		 * @param {Object} data
		 */
        sum: function (field, data) {
            var _self = this,
				records = data || _self.getResult(),
				sum = 0;
            BUI.each(records,
				function (record) {
				    var val = record[field];
				    if (!isNaN(val)) {
				        sum += parseFloat(val);
				    }
				});
            return sum;
        },
        /**
		 * 设置记录的值 ，触发 update 事件
		 * <pre><code>
		 *  store.setValue(obj,'value','new value');
		 * </code></pre>
		 * @param {Object} obj 修改的记录
		 * @param {String} field 修改的字段名
		 * @param {Object} value 修改的值
		 */
        setValue: function (obj, field, value) {
            var record = obj,
				_self = this;
            record[field] = value;
            if (!contains(record, _self.get("newRecords")) && !contains(record, _self.get("modifiedRecords"))) {
                _self.get("modifiedRecords").push(record);
            }
            _self.fire("update", {
                record: record,
                field: field,
                value: value
            });
        },
        _sortData: function (field, direction, data) {
            var _self = this;
            data = data || _self.getResult();
            _self.sortData(field, direction, data);
        },
        /**
		 * 结果集排序
		 * @param {Object} field
		 * @param {Object} direction
		 * @param {Object} records
		 */
        sortData: function (field, direction, records) {
            var _self = this,
				records = records || _self.getSortData();
            if (BUI.isArray(field)) {
                records = field;
                field = null;
            }
            field = field || _self.get("sortField");
            direction = direction || _self.get("sortDirection");
            _self.set("sortField", field);
            _self.set("sortDirection", direction);
            if (!field || !direction) {
                return records;
            }
            records.sort(function (obj1, obj2) {
                return _self.compare(obj1, obj2, field, direction);
            });
            return records;
        },
        /**
		 * 按照指定字段进行比较
		 * @param {Object} obj1
		 * @param {Object} obj2
		 * @param {Object} field
		 * @param {Object} direction ASC DESC
		 */
        compare: function (obj1, obj2, field, direction) {
            var _self = this,
				dir;
            field = field || _self.get("sortField");
            direction = direction || _self.get("sortDirection");
            if (!field || !direction) {
                return 1;
            }
            dir = direction === ASC ? 1 : -1;
            return _self.get("compareFunction")(obj1[field], obj2[field]) * dir;
        },
        /**
		 * 获取排序数据
		 */
        getSortData: function () {
            return this.getResult();
        },
        /**
		 * 设置结果集
		 * @param {Object} rows
		 * @param {Object} totalCount
		 */
        _setResult: function (rows, totalCount) {
            var _self = this,
				resultMap = _self.get("resultMap");
            totalCount = totalCount || rows.length;
            BUI.setValue(resultMap, _self.get("root"), rows);
            BUI.setValue(resultMap, _self.get("totalProperty"), totalCount);
            _self._clearChanges();
        },
        /**
		 * 清空所有改变
		 */
        _clearChanges: function () {
            var _self = this;
            BUI.Array.empty(_self.get("newRecords"));
            BUI.Array.empty(_self.get("modifiedRecords"));
            BUI.Array.empty(_self.get("deletedRecords"));
        },
        /**
		 * 获取匹配记录
		 * @param {Object} params
		 */
        _getMatches: function (params) {
            var _self = this,
				matchFields = _self.get("matchFields"),
				matchFn,
				data = _self.get("data") || [];
            if (params && matchFields.length) {
                matchFn = _self._getMatchFn(params, matchFields);
                data = BUI.Array.filter(data, matchFn);
            }
            return data;
        },
        /**
		 * 获取匹配函数
		 * @param {Object} params
		 * @param {Object} matchFields
		 */
        _getMatchFn: function (params, matchFields) {
            var _self = this;
            return function (obj) {
                var result = true;
                BUI.each(matchFields,
					function (field) {
					    if (params[field] != null && !(params[field] === obj[field])) {
					        result = false;
					        return false;
					    }
					});
                return result;
            };
        }
    });
    BUI.Data.Store = store;
})(window.BUI);
//BUI.Data.Node +
(function (BUI) {
    "use strict";

    function mapNode(cfg, map) {
        var rst = {};
        if (map) {
            BUI.each(cfg,
				function (v, k) {
				    var name = map[k] || k;
				    rst[name] = v;
				});
            rst.record = cfg;
        } else {
            rst = cfg;
        }
        return rst;
    }

    /**
	 * 树形数据结构的节点类
	 * @param {Object} cfg
	 * @param {Object} map
	 */
    function Node(cfg, map) {
        var _self = this;
        cfg = mapNode(cfg, map);
        BUI.mix(this, cfg);
    }
    BUI.augment(Node, {
        /**
		 * 是否根节点
		 * @type {Boolean}
		 */
        root: false,
        /**
		 * 是否叶子节点
		 * @type {Boolean}
		 */
        leaf: null,
        /**
		 * 显示节点时显示的文本
		 * @type {Object}
		 */
        text: '',
        /**
		 * 代表节点的编号
		 * @type {String}
		 */
        id: null,
        /**
		 * 子节点是否已经加载过
		 * @type {Boolean}
		 */
        loaded: false,
        /**
		 * 从根节点到此节点的路径，id的集合如： ['0','1','12'],
		 * 便于快速定位节点
		 * @type {Array}
		 */
        path: null,
        /**
		 * 父节点
		 * @type {BUI.Data.Node}
		 */
        parent: null,
        /**
		 * 树节点的等级
		 * @type {Number}
		 */
        level: 0,
        /**
		 * 节点是否由一条记录封装而成
		 * @type {Object}
		 */
        record: null,
        /**
		 * 子节点集合
		 * @type {BUI.Data.Node[]}
		 */
        children: null,
        /**
		 * 是否是Node对象
		 * @type {Object}
		 */
        isNode: true
    });
    BUI.Data.Node = Node;
})(window.BUI);
//BUI.Data.TreeStore
(function (BUI, $) {
    "use strict";
    var Node = BUI.Data.Node,
		Proxy = BUI.Data.Proxy;

    function TreeStore(config) {
        TreeStore.superclass.constructor.call(this, config);
        this._init();
    }
    TreeStore.ATTRS = {
        /**
        * 用户判断是否ajax异步数据
        **/
        url: {
        },
        /**
		 * 首付自动查询
		 */
        autoLoad: {
            value: false
        },
        /**
		 * 上次查询的参数
		 * @type {Object}
		 * @readOnly
		 */
        lastParams: {
            shared: false,
            value: {}
        },
        /**
		 * 参数
		 */
        params: {},
        /**
		 * 比较函数
		 * @cfg {Function} compareFunction
		 * 函数原型 function(v1,v2)，比较2个字段是否相等
		 * 如果是字符串则按照本地比较算法，否则使用 > ,== 验证
		 */
        matchFunction: {
            value: function (obj1, obj2) {
                return obj1 == obj2;
            }
        },
        /**
		 * 本地数据源
		 * @type {Array}
		 */
        data: {
            value: []
        },
        /**
		 *  根节点,初始化后不要更改对象，可以更改属性值
		 */
        root: {},
        /**
		 * 数据映射，用于设置的数据跟@see {BUI.Data.Node} 不一致时，进行匹配。
		 */
        map: {},
        /**
		 * 标示父元素id的字段名称
		 * @type {String}
		 */
        pidField: {},
        /**
		 * 返回数据标示数据的字段<br/>
		 * 异步加载数据时，返回数据可以使数组或者对象
		 * - 如果返回的是对象,可以附加其他信息,那么取对象对应的字段 {nodes : [],hasError:false}
		 * - 如何获取附加信息参看 @see {BUI.Data.AbstractStore-event-beforeprocessload}
		 */
        dataProperty: {
            value: "nodes"
        },
        events: {
            value: ["load", "add", "remove", "update"]
        },
        matchFields: {
            value: []
        }
    };
    BUI.extend(TreeStore, BUI.Base);
    BUI.augment(TreeStore, {
        isStore: true,
        _init: function () {
            var _self = this;
            _self.initRoot();
            _self._initParams();
            _self._initData();
        },
        /**
		 * 初始化根节点
		 */
        initRoot: function () {
            var _self = this,
				map = _self.get("map"),
				root = _self.get("root");
            if (!root) {
                root = {};
            }
            if (!root.isNode) {
                root = new Node(root, map);
            }
            root.path = [root.id];
            root.level = 0;
            if (root.children) {
                _self.setChildren(root, root.children);
            }
            _self.set("root", root);
        },
        /**
		 * 设置子节点
		 * @param {Object} node
		 * @param {Object} children
		 */
        setChildren: function (node, children) {
            var _self = this;
            node.children = [];
            if (!children.length) {
                return;
            }
            BUI.each(children,
				function (item) {
				    _self._add(item, node);
				});
        },
        /**
		 * 添加节点
		 * @param {Object} node 
		 * @param {Object} parent 父节点,如果未指定则为根节点
		 * @param {Object} index
		 */
        add: function (node, parent, index) {
            var _self = this;
            node = _self._add(node, parent, index);
            _self.fire("add", {
                node: node,
                record: node,
                index: index
            });
            return node;
        },
        _add: function (node, parent, index) {
            parent = parent || this.get("root");
            var _self = this,
				map = _self.get("map"),
				nodes = parent.children,
				nodeChildren;
            if (!node.isNode) {
                node = new Node(node, map);
            }
            nodeChildren = node.children || [];
            if (nodeChildren.length == 0 && node.leaf == null) {
                node.leaf = true;
            }
            if (parent) {
                parent.leaf = false;
            }
            node.parent = parent;
            node.level = parent.level + 1;
            node.path = parent.path.concat(node.id);
            index = index == null ? parent.children.length : index;
            BUI.Array.addAt(nodes, node, index);
            _self.setChildren(node, nodeChildren);
            return node;
        },
        _initParams: function () {
            var _self = this,
				lastParams = _self.get("lastParams"),
				params = _self.get("params");
            BUI.mix(lastParams, params);
        },
        _initData: function () {
            var _self = this,
				pidField = _self.get("pidField"),
				root = _self.get("root");
            if (!root.children) {
                _self.loadNode(root);
            }
        },
        /**
		 * 加载节点的子节点
		 * @param  {BUI.Data.Node} node 节点
		 * @param {Boolean} forceLoad 是否强迫重新加载节点，如果设置成true，不判断是否加载过
		 */
        loadNode: function (node, forceLoad) {
            var _self = this,
				pidField = _self.get("pidField"),
				params;
            if (!forceLoad && _self.isLoaded(node)) {
                return;
            }
            params = {
                id: node.id
            };
            if (pidField) {
                params[pidField] = node.id;
            }
            _self.load(params);
        },
        /**
		 * 根据特定参数进行查询
		 * @param {Object} params
		 * @param {Object} callback
		 */
        load: function (params, callback) {
            var _self = this,
				lastParams = _self.get("lastParams"),
                url = _self.get("url");
            BUI.mix(lastParams, params);
            if (url) {
                $.ajax({
                    cache: false,
                    url: url,
                    data: params,
                    type: "GET",
                    datatype: "json",
                    success: function (data) {
                        _self.onLoad(data, params);
                        if (callback) {
                            callback(data, params);
                        }
                    },
                    error: function (err) {
                        BUI.log(err.responseText);
                    }
                });
            } else {
                var data = _self._getMatches(params);
                _self.onLoad(data, params);

                if (callback) {
                    callback(data, params);
                }
            }
        },
        onLoad: function (data, params) {
            var _self = this,
				pidField = _self.get("pidField"),
				id = params.id || params[pidField],
				dataProperty = _self.get("dataProperty"),
				node = _self.findNode(id) || _self.get("root");

            if (BUI.isArray(data)) {
                _self.setChildren(node, data);
            } else {
                _self.setChildren(node, BUI.getValue(data, dataProperty));
            }
            node.loaded = true;
            _self.fire("load", {
                node: node,
                params: params
            });
        },
        /**
		 * 删除记录
		 * @param {Object} data
		 * @param {Object} match
		 */
        remove: function (node) {
            var parent = node.parent || _self.get("root"),
				index = BUI.Array.indexOf(node, parent.children);
            BUI.Array.remove(parent.children, node);
            if (parent.children.length === 0) {
                parent.leaf = true;
            }
            this.fire("remove", {
                node: node,
                record: node,
                index: index
            });
            node.parent = null;
            return node;
        },
        /**
		 * 更新记录
		 * @param {Object} node
		 * @param {Object} field
		 * @param {Object} value
		 */
        setValue: function (node, field, value) {
            var _self = this;
            node[field] = value;
            _self.fire("update", {
                node: node,
                record: node,
                field: field,
                value: value
            });
        },
        /**
		 * 更新记录
		 * @param {Object} node
		 */
        update: function (node) {
            this.fire("update", {
                node: node,
                record: node
            });
        },
        /**
		 * 获取当前缓存的纪录
		 */
        getResult: function () {
            return this.get("root").children;
        },
        /**
		 * 设置结果
		 * @param {Object} data
		 */
        setResult: function (data) {
            var _self = this,
				root = _self.get("root");
            _self.set("data", data);
            _self.load({
                id: root.id
            });
        },
        /**
		 * 查找节点
		 * <pre><code>
		 *  var node = store.findNode('1');//从根节点开始查找节点
		 *  
		 *  var subNode = store.findNode('123',node); //从指定节点开始查找
		 * </code></pre>
		 * @param  {String} id 节点Id
		 * @param  {BUI.Data.Node} [parent] 父节点
		 * @param {Boolean} [deep = true] 是否递归查找
		 * @return {BUI.Data.Node} 节点
		 */
        findNode: function (id, parent, deep) {
            return this.findNodeBy(function (node) {
                return node.id === id;
            },
				parent, deep);
        },
        /**
		 * 根据匹配函数查找节点
		 * @param  {Function} fn  匹配函数
		 * @param  {BUI.Data.Node} [parent] 父节点
		 * @param {Boolean} [deep = true] 是否递归查找
		 * @return {BUI.Data.Node} 节点
		 */
        findNodeBy: function (fn, parent, deep) {
            var _self = this;
            deep = deep == null ? true : deep;
            if (!parent) {
                var root = _self.get("root");
                if (fn(root)) {
                    return root;
                }
                return _self.findNodeBy(fn, root);
            }
            var children = parent.children,
				rst = null;
            BUI.each(children,
				function (item) {
				    if (fn(item)) {
				        rst = item;
				    } else if (deep) {
				        rst = _self.findNodeBy(fn, item);
				    }
				    if (rst) {
				        return false;
				    }
				});
            return rst;
        },
        /**
		 * 查找节点,根据匹配函数查找
		 * <pre><code>
		 *  var nodes = store.findNodesBy(function(node){
		 *   if(node.status == '0'){
		 *     return true;
		 *   }
		 *   return false;
		 *  });
		 * </code></pre>
		 * @param  {Function} func 匹配函数
		 * @param  {BUI.Data.Node} [parent] 父元素，如果不存在，则从根节点查找
		 * @return {Array} 节点数组
		 */
        findNodesBy: function (func, parent) {
            var _self = this,
				root, rst = [];
            if (!parent) {
                parent = _self.get("root");
            }
            BUI.each(parent.children,
				function (item) {
				    if (func(item)) {
				        rst.push(item);
				    }
				    rst = rst.concat(_self.findNodesBy(func, item));
				});
            return rst;
        },
        /**
		 * 根据path查找节点
		 * @return {BUI.Data.Node} 节点
		 * @ignore
		 */
        findNodeByPath: function (path) {
            if (!path) {
                return null;
            }
            var _self = this,
				root = _self.get("root"),
				pathArr = path.split(","),
				node,
				i,
				tempId = pathArr[0];
            if (!tempId) {
                return null;
            }
            if (root.id == tempId) {
                node = root;
            } else {
                node = _self.findNode(tempId, root, false);
            }
            if (!node) {
                return;
            }
            for (i = 1; i < pathArr.length; i = i + 1) {
                var tempId = pathArr[i];
                node = _self.findNode(tempId, node, false);
                if (!node) {
                    break;
                }
            }
            return node;
        },
        /**
		 * 是否包含指定节点，如果未指定父节点，从根节点开始搜索
		 * @param {Object} node
		 * @param {Object} parent
		 */
        contains: function (node, parent) {
            var _self = this,
				findNode = _self.findNode(node.id, parent);
            return !!findNode;
        },
        /**
		 * 是否包含数据
		 */
        hasData: function () {
            return this.get("root").children && this.get("root").children.length !== 0;
        },
        /**
		 * 是否已经加载过，叶子节点或者存在字节点的节点
		 * @param {Object} node
		 */
        isLoaded: function (node) {
            var root = this.get("root");
            if (node == root && !root.children) {
                return false;
            }
            if (!this.get("pidField")) {
                return true;
            }
            return node.loaded || node.leaf || !!(node.children && node.children.length);
        },
        /**
		 * 重新加载节点
		 * @param {Object} node
		 */
        reloadNode: function (node) {
            var _self = this;
            node = node || _self.get("root");
            node.loaded = false;
            _self.loadNode(node, true);
        },
        /**
		 * 加载节点，根据path
		 * @param {Object} path
		 */
        loadPath: function (path) {
            var _self = this,
				arr = path.split(","),
				id = arr[0];
            if (_self.findNodeByPath(path)) {
                return;
            }
            _self.load({
                id: id,
                path: path
            });
        },
        /**
		 * 获取匹配记录
		 * @param {Object} params
		 */
        _getMatches: function (params) {
            var _self = this,
				matchFields = _self.get("matchFields"),
				matchFn,
				data = _self.get("data") || [];
            if (params && matchFields.length) {
                matchFn = _self._getMatchFn(params, matchFields);
                data = BUI.Array.filter(data, matchFn);
            }
            return data;
        },
        /**
		 * 获取匹配参数
		 * @param {Object} params
		 * @param {Object} matchFields
		 */
        _getMatchFn: function (params, matchFields) {
            var _self = this;
            return function (obj) {
                var result = true;
                BUI.each(matchFields,
					function (field) {
					    if (params[field] != null && !(params[field] === obj[field])) {
					        result = false;
					        return false;
					    }
					});
                return result;
            };
        }
    });
    BUI.Data.TreeStore = TreeStore;
})(window.BUI, jQuery);