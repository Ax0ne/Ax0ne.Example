+ function() {
    var Utils = function() {
        this.formatterString = function() {}
    }
    /**
     * 格式化时间
     * @param  {Date} date 时间
     * @return {void}      
     */
    Utils.prototype.formatterDate = function(date) {
        return;
    }
    var utilsObject = {
    	/**
    	 * 转JSON对象
    	 * @param  {String} text JSON字符串格式
    	 * @param  {Function} func 回调函数
    	 * @return {Object}      返回的JSON字符串
    	 */
        toJSON: function (text, func) {
            return JSON.parse(text, func);
        },
        format: function (source, params) {
            /// <summary>
            /// 格式化第{n}个占位符
            /// </summary>
            /// <param name="source" type="String">
            /// 源字符串
            /// </param>
            /// <param name="params" type="String">
            /// 一个参数,或一个数组
            /// </param>
            /// <returns type="String" />
        }
    }
    Utils.prototype = utilsObject;
    window.sumsz = new Utils();
}()