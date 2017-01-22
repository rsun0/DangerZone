from flask import Flask, request, make_response, jsonify, render_template
import json
import os
import processing as p
#from pymongo import MongoClient

app = Flask(__name__)

# @app.route('/')
# def home():
#     return render_template('home.html')

@app.route('/request', methods=['POST'])
def send_danger():
    req = request.get_json(silent=True, force=True)
    lat = req.get("location").get("latitude")
    lon = req.get("location").get("longitude")
    print(lat)
    print(lon)
    
    here, north, south, east, west = p.dangerlevelcompass((lat,lon))
    
    notify = p.isdangerous(here)
    
    res = {
	    "danger": {
		    "here": here,
		    "north": north,
		    "south": south,
		    "east": east,
		    "west": west,
        },
        "notify": notify,
    }
    res = json.dumps(res, separators=(',',':'))
    # print(res)
    r = make_response(res)
    r.headers['Content-Type'] = 'application/json'
    return r
    
@app.route('/mapdata', methods=['GET'])
def send_map():
    with open('map.json') as data_file:    
        data = json.load(data_file)
    # wrapper = {'datapoints': data}
    data = json.dumps(data, separators=(',',':'))
    r = make_response(data)
    r.headers['Content-Type'] = 'application/json'
    return r

if __name__ == '__main__':
    port = int(os.getenv('PORT', 5000))
    app.run(debug=False, port=port, host='0.0.0.0')
    print("running and working hopefully")