require 'net/http'
require 'json'

# Login
uri = URI('http://developer.shipamax-api.com/api/users/login')
http = Net::HTTP.new(uri.host, uri.port)
req = Net::HTTP::Post.new(uri.path, 'Content-Type' => 'application/json')
req.body = {username: '[YOUR-USERNAME]', password: '[YOUR-PASSWORD]'}.to_json
res = http.request(req)
puts "response #{res.body}"
puts "Test complete"
