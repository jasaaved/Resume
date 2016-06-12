import http.client
import urllib.request
import urllib.error

class ACCOUNTNOTFOUNDERROR(Exception):
    '''
    This error is raised when the _get_data() function is not able to connect.
    '''
    pass

def _get_data(Username:str)->str:
    '''
    This function uses the username the user has inputed in the main game,
    creates a url, and returns the data by indexing the data that is needed.
    '''
    url = 'http://128.195.54.59:8004/DataWarehouse/requestData?id={}&timeInterval=24'.format(Username)
    try:    
        response = urllib.request.urlopen(url)
        line = str(response.readline().strip())
        last_index = line.find('lat')
        first_index = line.find('"activity":')
        line = (line[first_index+12:last_index-3]).replace('"', '')
        data_list = line.split(',')
        if len(data_list) == 1 and '' in data_list:
            raise ACCOUNTNOTFOUNDERROR
        return data_list
    
    except:
        raise ACCOUNTNOTFOUNDERROR



        
