
export function beforeRegistration({ Vue, config, store, isServer }) {
  if (!Vue.prototype.$isServer) {

    let jsUrl = 'https://www.paypalobjects.com/api/checkout.js'
    let docHead = document.getElementsByTagName('head')[0]
    let docScript = document.createElement('script')
    docScript.type = 'text/javascript'
    docScript.src = jsUrl
    docHead.appendChild(docScript)
  }
}
