/**=========================================================
 * Module: constants.js
 * Define constants to inject across the application
 =========================================================*/
App
  .constant('APP_COLORS', {
      'primary': '#5d9cec',
      'success': '#27c24c',
      'info': '#23b7e5',
      'warning': '#ff902b',
      'danger': '#f05050',
      'inverse': '#131e26',
      'green': '#37bc9b',
      'pink': '#f532e5',
      'purple': '#7266ba',
      'dark': '#3a3f51',
      'yellow': '#fad732',
      'gray-darker': '#232735',
      'gray-dark': '#3a3f51',
      'gray': '#dde6e9',
      'gray-light': '#e4eaec',
      'gray-lighter': '#edf1f2'
  })
  .constant('APP_MEDIAQUERY', {
      'desktopLG': 1200,
      'desktop': 992,
      'tablet': 768,
      'mobile': 480
  })
  .constant('APP_REQUIRES', {
      // jQuery based and standalone scripts
      scripts: {
          'whirl': ['vendor/whirl/dist/whirl.css'],
          'modernizr': ['vendor/modernizr/modernizr.js'],
          'icons': ['vendor/fontawesome/css/font-awesome.min.css',
                                 'vendor/simple-line-icons/css/simple-line-icons.css'],
          'lodash': ['http://cdnjs.cloudflare.com/ajax/libs/lodash.js/3.10.0/lodash.min.js'],
          //'amcharts': ['Content/vendor/amcharts/amcharts.js', 'Content/vendor/amcharts/serial.js', 'Content/vendor/amcharts/themes/light.js']


      },
      // Angular based script (use the right module name)
      modules: [
          { name: 'isteven-multi-select', files: ['Scripts/isteven-multi-select.js'] },
          {
              name: 'ngDialog', files: ['vendor/ngDialog/js/ngDialog.min.js',
                                                     'vendor/ngDialog/css/ngDialog.min.css',
                                                     'vendor/ngDialog/css/ngDialog-theme-default.min.css']
          }
        // { name: 'toaster', files: ['vendor/angularjs-toaster/toaster.js','vendor/angularjs-toaster/toaster.css'] }
      ]

  })
;