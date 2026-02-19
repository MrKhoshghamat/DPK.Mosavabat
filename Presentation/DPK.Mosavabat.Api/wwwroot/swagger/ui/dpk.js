var dpk = dpk || {};
(function () {

    /* Application paths *****************************************/

    //Current application root path (including virtual directory if exists).	
    dpk.appPath = dpk.appPath || '/';

    /* UTILS ***************************************************/

    dpk.utils = dpk.utils || {};

    /**	
        * Sets a cookie value for given key.	
        * This is a simple implementation created to be used by DPK.	
        * Please use a complete cookie library if you need.	
        * @param {string} key	
        * @param {string} value	
        * @param {Date} expireDate (optional). If not specified the cookie will expire at the end of session.	
        * @param {string} path (optional)	
        */
    dpk.utils.setCookieValue = function (key, value, expireDate, path) {
        var cookieValue = encodeURIComponent(key) + '=';

        if (value) {
            cookieValue = cookieValue + encodeURIComponent(value);
        }

        if (expireDate) {
            cookieValue = cookieValue + "; expires=" + expireDate.toUTCString();
        }

        if (path) {
            cookieValue = cookieValue + "; path=" + path;
        }

        document.cookie = cookieValue;
    };

    /**	
    * Gets a cookie with given key.	
    * This is a simple implementation created to be used by DPK.	
    * Please use a complete cookie library if you need.	
    * @param {string} key	
    * @returns {string} Cookie value or null	
    */
    dpk.utils.getCookieValue = function (key) {
        var equalities = document.cookie.split('; ');
        for (var i = 0; i < equalities.length; i++) {
            if (!equalities[i]) {
                continue;
            }

            var splitted = equalities[i].split('=');
            if (splitted.length != 2) {
                continue;
            }

            if (decodeURIComponent(splitted[0]) === key) {
                return decodeURIComponent(splitted[1] || '');
            }
        }

        return null;
    };

    /**	
    * Deletes cookie for given key.	
    * This is a simple implementation created to be used by DPK.	
    * Please use a complete cookie library if you need.	
    * @param {string} key	
    * @param {string} path (optional)	
    */
    dpk.utils.deleteCookie = function (key, path) {
        var cookieValue = encodeURIComponent(key) + '=';

        cookieValue = cookieValue + "; expires=" + (new Date(new Date().getTime() - 86400000)).toUTCString();

        if (path) {
            cookieValue = cookieValue + "; path=" + path;
        }

        document.cookie = cookieValue;
    }

    /* SECURITY ***************************************/
    dpk.security = dpk.security || {};
    dpk.security.antiForgery = dpk.security.antiForgery || {};

    dpk.security.antiForgery.tokenCookieName = 'XSRF-TOKEN';
    dpk.security.antiForgery.tokenHeaderName = 'X-XSRF-TOKEN';

    dpk.security.antiForgery.getToken = function () {
        return dpk.utils.getCookieValue(dpk.security.antiForgery.tokenCookieName);
    };

})();