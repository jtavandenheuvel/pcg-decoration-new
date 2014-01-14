#include "cpp_file.h"  

#include <iostream>

#include<boost\chrono.hpp>
#include<boost/shared_ptr.hpp>

#include<CGAL/Exact_predicates_inexact_constructions_kernel.h>
#include<CGAL/Polygon_with_holes_2.h>
#include<CGAL/create_straight_skeleton_from_polygon_with_holes_2.h>
#include<CGAL/create_offset_polygons_2.h>

#include "print.h"


typedef CGAL::Exact_predicates_inexact_constructions_kernel K ;
typedef K::Point_2                   Point ;
typedef CGAL::Polygon_2<K>           Polygon_2 ;
typedef CGAL::Polygon_with_holes_2<K> Polygon_with_holes ;
typedef boost::shared_ptr<Polygon_2> PolygonPtr ;
typedef CGAL::Straight_skeleton_2<K> Ss ;
typedef boost::shared_ptr<Ss> SsPtr ;
typedef std::vector<PolygonPtr> PolygonPtrVector ;
typedef std::vector< boost::shared_ptr< CGAL::Polygon_2<K> > > PolygonVector ;

cpp_file::cpp_file(void)  
{  
}  
  
cpp_file::~cpp_file(void)  
{  
}  
  
//input = polygon
//input2 = holes
//output = SS
//output2 = offset
void cpp_file::SSAwithoutHoles(float* input, int length, float* output,float* output2, float offSet)  
{ 
	SSAwithHoles(input, length, input, 0, 0, output, output2, offSet);
}
//input = polygon
//input2 = holes
//output = SS
//output2 = offset
void cpp_file::SSAwithHoles(float* input,int length,float* input2, int* length2, int totalHoles, float* output, float* output2, float offSet)  
{ 
  //auto start = boost::chrono::high_resolution_clock::now();

  Polygon_2 outer ;
  for (int n=0; n < length; n=n+2) {
	  outer.push_back( Point(input[n],input[n+1]) ) ;
  }
  Polygon_with_holes poly( outer ) ;

  int n = 0;
  for(int i = 0; i < totalHoles; i++){
	Polygon_2 hole ;
	
	for (int x = 0; x < length2[i]; x++, n=n+2) {
	  hole.push_back( Point(input2[n],input2[n+1]) ) ;
	}
	poly.add_hole( hole ) ;
  }

  // Calculate straight skeleton
  SsPtr iss = CGAL::create_interior_straight_skeleton_2(poly);

  // Set data in output array
  typedef CGAL::Straight_skeleton_2<K> Ss ;
  typedef Ss::Halfedge_const_iterator Halfedge_const_iterator ;

  // Count size for output array
  int count = 0;
  for ( Halfedge_const_iterator i = iss->halfedges_begin(); i != iss->halfedges_end(); ++i )
  {
	  if(i->is_bisector())
	  {
		output[count++] = i->opposite()->vertex()->point().x();
		output[count++] = i->opposite()->vertex()->point().y();
		output[count++] = i->vertex()->point().x();
		output[count++] = i->vertex()->point().y();
	  }
  }

   
  double lOffset = offSet;
  int currentPolyCount = 0;
  count = 0;

  PolygonPtrVector offset_polygons = CGAL::create_offset_polygons_2<Polygon_2>(lOffset,*iss);

  for( PolygonVector::const_iterator pi = offset_polygons.begin() ; pi != offset_polygons.end() ; ++ pi )
  {
	typedef CGAL::Polygon_2<K> Polygon ;
	CGAL::Polygon_2<K> const& polyOffset = **pi;
	for( Polygon::Vertex_const_iterator vi = polyOffset.vertices_begin() ; vi != polyOffset.vertices_end() ; ++ vi )
	{
		output2[count++] = vi->x();
		output2[count++] = vi->y();
		if(count > 2+currentPolyCount)
		{
			output2[count++] = output2[count-2];
			output2[count++] = output2[count-2];
		}
	}  
	output2[count++] = output2[currentPolyCount];
	output2[count++] = output2[currentPolyCount+1];
	currentPolyCount = count;
  }
  //auto end = boost::chrono::high_resolution_clock::now();
  //auto elapsed = boost::chrono::duration_cast<boost::chrono::milliseconds>(end - start);

  //std::cout << "Elapsed Time: " << elapsed << "\n"; // prints Output sentence on screen
}  
