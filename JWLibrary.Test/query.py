################## parameters #################
#ironpython version 2.7 (python version 2.7)
#name="seokwon"
#age=18
#v=''
###############################################
import io

sql = io.StringIO()
sql.write('SELECT * FROM CUSTOMER WHERE 1=1')

if name != '':
    sql.write(''' AND NAME = '{}' '''.format(name))
    
if age > 0:
    sql.write(''' AND AGE = {} '''.format(age))
    
if v != '':
    sql.write(''' AND VALUE = '{}' '''.format(v))
    
resultSql = sql.getvalue()
sql.close()
    
